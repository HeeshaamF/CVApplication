using CVApplication.Models;
using CVApplication.Services.Interfaces;
using CVApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class AnalyseController : Controller
{
    private readonly IAnalyseService _analyseService;
    private readonly IOffreEmploiService _offreEmploiService;

    public AnalyseController(IAnalyseService analyseService, IOffreEmploiService offreEmploiService)
    {
        _analyseService = analyseService;
        _offreEmploiService = offreEmploiService;
    }

    [HttpGet]
    public async Task<IActionResult> Details(int cvId, int? offreId)
    {
        var analyses = _analyseService.GetAnalysesByCVId(cvId);

        if (!analyses.Any())
        {
            var cv = _analyseService.GetCVById(cvId);

            if (cv == null)
                return NotFound();

            OffreEmploi? offre = null;

            if (offreId.HasValue)
            {
                offre = await _offreEmploiService.GetById(offreId.Value);
            }

            var analyse = _analyseService.AnalyserCV(cv, offre);

            await _analyseService.SaveAnalyse(analyse);

            analyses = new List<AnalyseCV> { analyse };
        }

        var model = new AnalyseCVViewModel
        {
            CV = analyses.First().CV,
            Analyses = analyses
        };

        return View(model);
    }
    
    [HttpGet]
    public async Task<IActionResult> Comparer(int cvId)
    {
        var offres = await _offreEmploiService.VoirListe();

        var model = new ComparaisonCVViewModel
        {
            CVId = cvId,
            Offres = offres.ToList()
        };

        return View(model);
    }
    
    [HttpGet]
    public async Task<IActionResult> ComparerResult(int cvId, int offreId)
    {
        var cv = _analyseService.GetCVById(cvId);
        var offre = await _offreEmploiService.GetById(offreId);

        if (cv == null || offre == null)
            return NotFound();

        var analyses = _analyseService.GetAnalysesByCVId(cvId);

        var analyse = analyses.FirstOrDefault(a => a.OffreEmploiId == offreId);

        if (analyse == null)
        {
            analyse = _analyseService.AnalyserCV(cv, offre);
            await _analyseService.SaveAnalyse(analyse);
        }

        var model = new ResultatMatchingViewModel
        {
            CV = cv,
            Offre = offre,
            ScoreMatching = analyse.ScoreMatching
        };

        return View(model);
    }
    
    public async Task<IActionResult> MesAnalyses()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Challenge();

        var analyses = await _analyseService.GetHistoriqueAnalyses(userId);

        return View(analyses);
    }
    
    [HttpGet]
    public async Task<IActionResult> Comparaisons(int cvId)
    {
        var cv = _analyseService.GetCVById(cvId);
        if (cv == null) return NotFound();

        var analyses = _analyseService.GetAnalysesByCVId(cvId)
            .Where(a => a.OffreEmploiId != null)
            .ToList();

        var model = new AnalyseCVViewModel
        {
            CV = cv,
            Analyses = analyses
        };

        return View(model);
    }

}