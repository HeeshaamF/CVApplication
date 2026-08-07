using System.ComponentModel.DataAnnotations;

namespace CVApplication.ViewModels;

public class LoginViewModel {
    [Required(ErrorMessage = "L'adresse mail est obligatoire.")]
    [EmailAddress(ErrorMessage = "Veuillez entrer une adresse mail valide.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}