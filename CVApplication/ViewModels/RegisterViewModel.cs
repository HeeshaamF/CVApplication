using System.ComponentModel.DataAnnotations;

namespace CVApplication.ViewModels;

public class RegisterViewModel {
    [Required(ErrorMessage = "Le nom est obligatoire.")]
    public string Nom { get; set; }
    
    [Required(ErrorMessage = "L'adresse mail est obligatoire.")]
    [EmailAddress(ErrorMessage = "Veuillez entrer une adresse mail valide.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères, dont une minuscule, une majuscule et un chiffre.")]
    [DataType(DataType.Password)]
    public string MotDePasse { get; set; }
    
    [Required(ErrorMessage = "La confirmation du mot de passe est obligatoire.")]
    [Compare("MotDePasse", ErrorMessage = "Les mots de passe ne correspondent pas.")]
    [DataType(DataType.Password)]
    public string ConfirmMotDePasse { get; set; }
}
