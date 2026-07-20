using CVApplication.Helpers;
using CVApplication.Models;
using CVApplication.Services.Interfaces;

namespace CVApplication.Services;

public class ScoreService : IScoreService
{
    public int EvaluerStructure(AnalyseResultat analyse)
    {
        int rubriquesPresentes = 0;
        int totalRubriques = 4;

        if (analyse.HasProfil) rubriquesPresentes++;
        if (analyse.HasFormation) rubriquesPresentes++;
        if (analyse.HasExperience) rubriquesPresentes++;
        if (analyse.HasContact) rubriquesPresentes++;

        return (int)Math.Round((double)rubriquesPresentes / totalRubriques * 100);
    }

    public int EvaluerCompetences(AnalyseResultat analyse)
    {
        int score = 0;

        // Hard skills
        int hardCount = analyse.Competences.Count(c => SkillKeywords.HardSkills.Contains(c.ToLowerInvariant()));
        score += hardCount * ScoreWeights.HardSkill;   // chaque hard skill = +10

        // Soft skills
        int softCount = analyse.Competences.Count(c => SkillKeywords.SoftSkills.Contains(c.ToLowerInvariant()));
        score += softCount * ScoreWeights.SoftSkill;    // chaque soft skill = +3

        // Langues avec niveaux
        foreach (var langue in analyse.Langues)
        {
            bool hasNiveau = SkillKeywords.NiveauxLangue.Any(n => langue.ToLowerInvariant().Contains(n));

            if (hasNiveau)
                score += ScoreWeights.LangueAvecNiveau; // langue avec niveau = +10
            else
                score += ScoreWeights.LangueSansNiveau;  // langue sans niveau = +5
        }

        // Limite max
        return Math.Min(score, 100);
    }
    
    public int EvaluerLisibilite(string texte)
    {
        int score = 100;

        if (string.IsNullOrWhiteSpace(texte))
            return 0;

        // pénalités simples (logique réaliste)
        if (texte.Length < 300)
            score -= 30;

        if (!texte.Contains("\n"))
            score -= 10;

        if (texte.Count(char.IsUpper) < 10)
            score -= 10;

        return Math.Max(score, 0);
    }

    public int CalculerScore(AnalyseResultat analyse, string texte)
    {
        int structure = EvaluerStructure(analyse);
        int competences = EvaluerCompetences(analyse);
        int lisibilite = EvaluerLisibilite(texte);

        return (int)(structure * 0.4 + competences * 0.4 + lisibilite * 0.2);
    }
}