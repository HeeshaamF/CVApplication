using System.ComponentModel.DataAnnotations;

namespace CVApplication.ViewModels;

public class RegisterViewModel {
    [Required(ErrorMessage = "Le nom est obligatoire.")]
    public string Nom { get; set; }
    
    [Required(ErrorMessage = "L'adresse mail est obligatoire.")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
    [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères.")]
    [DataType(DataType.Password)]
    public string MotDePasse { get; set; }
    
    [Required(ErrorMessage = "La confirmation du mot de passe est obligatoire.")]
    [Compare("MotDePasse", ErrorMessage = "Les mots de passe ne correspondent pas.")]
    [DataType(DataType.Password)]
    public string ConfirmMotDePasse { get; set; }
}
