using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CVApplication.ViewModels;

public class RegisterViewModel {
    [Required(ErrorMessage = "Le nom est obligatoire.")]
    public string Nom { get; set; }
    
    [Required(ErrorMessage = "L'adresse mail est obligatoire.")]
    [EmailAddress(ErrorMessage = "Veuillez entrer une adresse mail valide.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
    [DataType(DataType.Password)]
    [CustomValidation(typeof(RegisterViewModel), nameof(ValidatePassword))]
    public string MotDePasse { get; set; }
    
    [Required(ErrorMessage = "La confirmation du mot de passe est obligatoire.")]
    [Compare("MotDePasse", ErrorMessage = "Les mots de passe ne correspondent pas.")]
    [DataType(DataType.Password)]
    public string ConfirmMotDePasse { get; set; }

    // Validation personnalisée
    public static ValidationResult? ValidatePassword(string? password, ValidationContext context)
    {
        if (string.IsNullOrEmpty(password))
            return new ValidationResult("Le mot de passe est obligatoire.");

        var errors = new List<string>();

        if (password.Length < 8)
            errors.Add("au moins 8 caractères");
        if (!Regex.IsMatch(password, "[a-z]"))
            errors.Add("une minuscule");
        if (!Regex.IsMatch(password, "[A-Z]"))
            errors.Add("une majuscule");
        if (!Regex.IsMatch(password, "[0-9]"))
            errors.Add("un chiffre");

        if (errors.Any())
        {
            return new ValidationResult(
                "Le mot de passe doit contenir " + string.Join(", ", errors) + "."
            );
        }

        return ValidationResult.Success;
    }
}