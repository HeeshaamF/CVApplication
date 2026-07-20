using Microsoft.AspNetCore.Mvc;
using CVApplication.Services.Interfaces;

namespace CVApplication.Controllers;

public class RapportController : Controller
{
    private readonly IAnalyseService _analyseService;
    private readonly IRecommandationService _recommandationService;
    private readonly IRapportService _rapportService;
    private readonly IWebHostEnvironment _environment;

    public RapportController(
        IAnalyseService analyseService,
        IRecommandationService recommandationService,
        IRapportService rapportService,
        IWebHostEnvironment environment)
    {
        _analyseService = analyseService;
        _recommandationService = recommandationService;
        _rapportService = rapportService;
        _environment = environment;
    }

    [HttpGet]
    public IActionResult Telecharger(int analyseId)
    {
        var analyse = _analyseService.GetAnalyseById(analyseId);

        if (analyse == null || analyse.CV == null)
            return NotFound();

        // Construire chemin complet
        var cheminComplet = Path.Combine(_environment.WebRootPath, analyse.CV.Chemin);

        // Extraire texte
        var texte = _analyseService.ExtraireTexte(cheminComplet);

        // Générer recommandations
        var resultat = _analyseService.DetecterRubriques(texte);
        var conseils = _recommandationService.GenererConseils(resultat);

        // Générer rapport
        var rapport = _rapportService.GenererRapport(analyse, conseils);

        // Exporter PDF
        var pdfBytes = _rapportService.ExporterPDF(rapport);

        return File(pdfBytes, "application/pdf", $"Rapport_{analyse.CV.Nom}.pdf");
    }
}