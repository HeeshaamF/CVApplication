using CVApplication.Data;
using CVApplication.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using CVApplication.Models;

namespace CVApplication.Services;

public class CVService : ICVService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public CVService(
        ApplicationDbContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<CV?> GetCVById(int id)
    {
        return await _context.CVs.FindAsync(id);
    }

    public async Task<CV> UploadCV(IFormFile fichier, string userId)
    {
        Console.WriteLine("Début UploadCV");
        if (fichier == null || fichier.Length == 0)
            throw new ArgumentException("Aucun fichier.");

        var extension = Path.GetExtension(fichier.FileName).ToLowerInvariant();

        var extensionsAutorisees = new[]
        {
            ".pdf",
            ".docx"
        };

        if (!extensionsAutorisees.Contains(extension))
        {
            throw new InvalidOperationException("Le fichier doit être au format PDF (.pdf) ou Word (.docx).");
        }

        var dossier = Path.Combine(_environment.WebRootPath, "uploads", "cv");

        Directory.CreateDirectory(dossier);

        var nomFichier = $"{Guid.NewGuid()}{extension}";

        var cheminComplet = Path.Combine(dossier, nomFichier);

        using (var stream = new FileStream(cheminComplet, FileMode.Create))
        {
            await fichier.CopyToAsync(stream);
        }

        var cv = new CV
        {
            UserId = userId,
            Nom = fichier.FileName,
            Chemin = Path.Combine("uploads", "cv", nomFichier),
            DateUpload = DateTime.UtcNow,
            ScoreGlobal = 0
        };
        
        _context.CVs.Add(cv);

        await _context.SaveChangesAsync();

        return cv;
    }
    
    
    public async Task<IEnumerable<CV>> GetCVsByUser(string userId)
    {
        return await _context.CVs
            .Include(cv => cv.AnalysesCV)
            .Where(cv => cv.UserId == userId)
            .OrderByDescending(cv => cv.DateUpload)
            .ToListAsync();
    }
}