using CVApplication.Data;
using CVApplication.Helpers;
using CVApplication.Models;
using CVApplication.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Packaging;
using UglyToad.PdfPig;
using System.Text.RegularExpressions;
namespace CVApplication.Services;

public class AnalyseService : IAnalyseService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly IScoreService _scoreService;
    private readonly IMatchingService _matchingService;

    public AnalyseService(ApplicationDbContext context, IWebHostEnvironment environment, IScoreService scoreService, IMatchingService matchingService)
    {
        _context = context;
        _environment = environment;
        _scoreService = scoreService;
        _matchingService = matchingService;
    }
    
    public CV? GetCVById(int id)
    {
        return _context.CVs
            .Include(c => c.AnalysesCV)
            .FirstOrDefault(c => c.Id == id);
    }
    
    public AnalyseResultat DetecterRubriques(string texte)
    {
        // Normalisation : tout en minuscules et suppression des espaces 
        string texteMin = Regex.Replace(texte.ToLowerInvariant(), @"\s+", " ");

        var resultat = new AnalyseResultat
        {
            HasProfil = RubriqueKeywords.ProfilRegex.IsMatch(texteMin),
            HasFormation = RubriqueKeywords.FormationRegex.IsMatch(texteMin),
            HasExperience = RubriqueKeywords.ExperienceRegex.IsMatch(texteMin),
            HasCompetences = RubriqueKeywords.CompetencesRegex.IsMatch(texteMin),
            // Détection des langues même sans rubrique 
            HasLangues = RubriqueKeywords.LanguesRegex.IsMatch(texteMin) || SkillKeywords.LanguesConnues.Any(l => texteMin.Contains(l.ToLowerInvariant())),
            HasContact = RubriqueKeywords.ContactRegex.IsMatch(texteMin),
            Competences = new List<string>(),
            Langues = new List<string>()
        };

        // Détection des compétences techniques
        foreach (var skill in SkillKeywords.HardSkills)
        {
            if (texteMin.Contains(skill.ToLowerInvariant()))
                resultat.Competences.Add(skill);
        }
        
        // Détection des soft skills
        foreach (var skill in SkillKeywords.SoftSkills)
        {
            if (texteMin.Contains(skill.ToLowerInvariant()))
                resultat.Competences.Add(skill);
        }
        
        // Détection des langues avec niveaux
        foreach (var lang in SkillKeywords.LanguesConnues)
        {
            if (texteMin.Contains(lang.ToLowerInvariant()))
            {
                var niveauxTrouves = SkillKeywords.NiveauxLangue
                    .Where(n => texteMin.Contains(n.ToLowerInvariant()))
                    .ToList();

                if (niveauxTrouves.Any())
                {
                    foreach (var niveau in niveauxTrouves)
                        resultat.Langues.Add($"{lang} {niveau.ToUpperInvariant()}");
                }
                else
                {
                    resultat.Langues.Add(lang);
                }
            }
        }
        return resultat;
    }

    public AnalyseCV AnalyserCV(CV cv, OffreEmploi? offre)
    {
        var cheminComplet = Path.Combine(_environment.WebRootPath, cv.Chemin);

        string texte = ExtraireTexte(cheminComplet);

        var resultat = DetecterRubriques(texte);

        double scoreStructure = _scoreService.EvaluerStructure(resultat);
        double scoreCompetences = _scoreService.EvaluerCompetences(resultat);
        double scoreLisibilite = _scoreService.EvaluerLisibilite(texte);

        double scoreGlobal = _scoreService.CalculerScore(resultat, texte);

        cv.ScoreGlobal = scoreGlobal;

        // Sauvegarde du score directement en base
        _context.CVs.Update(cv);
        _context.SaveChanges();

        double scoreMatching = 0;

        if (offre != null)
        {
            scoreMatching = _matchingService.CalculerMatching(texte, offre);
        }

        return new AnalyseCV
        {
            CVId = cv.Id,
            CV = cv,
            OffreEmploiId = offre?.Id,
            OffreEmploi = offre,
            ScoreStructure = scoreStructure,
            ScoreCompetences = scoreCompetences,
            ScoreLisibilite = scoreLisibilite,
            ScoreMatching = scoreMatching
        };
    }


    public AnalyseCV? GetAnalyseById(int id)
    {
        return _context.AnalyseCVs
            .Include(a => a.CV)
            .Include(a => a.Recommandations)
            .Include(a => a.OffreEmploi)
            .FirstOrDefault(a => a.Id == id);
    }
    
    public List<AnalyseCV> GetAnalysesByCVId(int cvId)
    {
        return _context.AnalyseCVs
            .Include(a => a.Recommandations)
            .Include(a => a.CV)
            .Include(a => a.OffreEmploi) 
            .Where(a => a.CVId == cvId)
            .ToList();
    }

    
    private string ExtraireTextePdf(string chemin)
    {
        var texte = "";

        using (var document = PdfDocument.Open(chemin))
        {
            foreach (var page in document.GetPages())
            {
                texte += page.Text + Environment.NewLine;
            }
        }

        return texte;
    }
    
    private string ExtraireTexteDocx(string chemin)
    {
        using var document = WordprocessingDocument.Open(chemin, false);

        return document.MainDocumentPart?
                   .Document
                   .Body?
                   .InnerText
               ?? "";
    }
    
    public string ExtraireTexte(string chemin)
    {
        var extension = Path.GetExtension(chemin).ToLowerInvariant();

        switch (extension)
        {
            case ".pdf":
                return ExtraireTextePdf(chemin);

            case ".docx":
                return ExtraireTexteDocx(chemin);

            default:
                throw new InvalidOperationException("Format non supporté.");
        }
    }
    
    public async Task SaveAnalyse(AnalyseCV analyse)
    {
        _context.AnalyseCVs.Add(analyse);

        _context.CVs.Update(analyse.CV);

        await _context.SaveChangesAsync();
    }
    
    public async Task<List<AnalyseCV>> GetHistoriqueAnalyses(string userId)
    {
        return await _context.AnalyseCVs
            .Include(a => a.CV)
            .Include(a => a.OffreEmploi)
            .Where(a => a.CV.UserId == userId)
            .OrderByDescending(a => a.CV.DateUpload)
            .ToListAsync();
    }
}