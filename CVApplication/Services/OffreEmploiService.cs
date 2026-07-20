using CVApplication.Data;
using CVApplication.Models;
using CVApplication.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CVApplication.Services;

public class OffreEmploiService : IOffreEmploiService
{
    private readonly ApplicationDbContext _context;

    public OffreEmploiService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OffreEmploi>> VoirListe()
    {
        return await _context.OffresEmploi
            .OrderBy(o => o.Titre)
            .ToListAsync();
    }

    public async Task<OffreEmploi?> GetById(int id)
    {
        return await _context.OffresEmploi.FindAsync(id);
    }

    public async Task AjouterOffre(OffreEmploi offre)
    {
        _context.OffresEmploi.Add(offre);
        await _context.SaveChangesAsync();
    }

    public async Task ModifierOffre(OffreEmploi offre)
    {
        _context.OffresEmploi.Update(offre);
        await _context.SaveChangesAsync();
    }

    public async Task SupprimerOffre(int id)
    {
        var offre = await _context.OffresEmploi.FindAsync(id);

        if (offre != null)
        {
            _context.OffresEmploi.Remove(offre);
            await _context.SaveChangesAsync();
        }
    }
}