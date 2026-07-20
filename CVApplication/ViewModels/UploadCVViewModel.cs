using System.ComponentModel.DataAnnotations;

namespace CVApplication.ViewModels;

public class UploadCVViewModel
{
    [Required]
    public IFormFile Fichier { get; set; }
}