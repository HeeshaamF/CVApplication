using CVApplication.Models;

namespace CVApplication.Services.Interfaces;

public interface ICVService
{
    Task<CV> UploadCV(
        IFormFile fichier,
        string userId);

    Task<CV?> GetCVById(int id);

    Task<IEnumerable<CV>> GetCVsByUser( string userId);
    
}