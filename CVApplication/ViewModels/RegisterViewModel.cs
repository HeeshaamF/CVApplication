using System.ComponentModel.DataAnnotations;

namespace CVApplication.ViewModels;

public class RegisterViewModel {
    [Required]
    public string Nom { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères.")]
    [DataType(DataType.Password)]
    public string MotDePasse { get; set; }
    
    [Required]
    [Compare("MotDePasse", ErrorMessage = "Les mots de passe ne correspondent pas.")]
    [DataType(DataType.Password)]
    public string ConfirmMotDePasse { get; set; }
}
