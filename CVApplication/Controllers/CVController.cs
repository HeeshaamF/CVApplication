using System.Security.Claims;
using CVApplication.Services.Interfaces;
using CVApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class CVController : Controller
{
    private readonly ICVService _cvService;
    private readonly IAnalyseService _analyseService;

    public CVController(ICVService cvService, IAnalyseService analyseService)
    {
        _cvService = cvService;
        _analyseService = analyseService;
    }
    
    [HttpGet]
    public IActionResult UploadCV()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadCV(UploadCVViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        try
        {
            // Étape 1 : Upload du CV
            var cv = await _cvService.UploadCV(model.Fichier, userId);

            // Étape 2 : Analyse immédiate du CV
            var analyse = _analyseService.AnalyserCV(cv, null);

            TempData["Success"] = "Le CV a été déposé et analysé avec succès.";

            return RedirectToAction(nameof(MesCV));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "Une erreur est survenue lors du dépôt du CV.");
            return View(model);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> MesCV()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var cvs = await _cvService.GetCVsByUser(userId);

        return View(cvs);
    }
}
