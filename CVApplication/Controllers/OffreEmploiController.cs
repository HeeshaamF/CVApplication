using CVApplication.Models;
using CVApplication.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVApplication.Controllers;

[Authorize(Roles = "Admin")]
public class OffreEmploiController : Controller
{
    private readonly IOffreEmploiService _offreService;

    public OffreEmploiController(IOffreEmploiService offreService)
    {
        _offreService = offreService;
    }

    public async Task<IActionResult> Index()
    {
        var offres = await _offreService.VoirListe();

        return View(offres);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OffreEmploi offre)
    {
        if (!ModelState.IsValid)
            return View(offre);

        await _offreService.AjouterOffre(offre);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var offre = await _offreService.GetById(id);

        if (offre == null)
            return NotFound();

        return View(offre);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(OffreEmploi offre)
    {
        if (!ModelState.IsValid)
            return View(offre);

        await _offreService.ModifierOffre(offre);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var offre = await _offreService.GetById(id);

        if (offre == null)
            return NotFound();

        return View(offre);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _offreService.SupprimerOffre(id);

        return RedirectToAction(nameof(Index));
    }
}