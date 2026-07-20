using System.ComponentModel.DataAnnotations;

namespace CVApplication.Models;

public class OffreEmploi
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le titre est obligatoire.")]
    [StringLength(100)]
    [Display(Name = "Titre")]
    public string Titre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La description est obligatoire.")]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Les compétences attendues sont obligatoires.")]
    [Display(Name = "Compétences attendues")]
    public string CompetencesAttendues { get; set; } = string.Empty;

    public ICollection<AnalyseCV> Analyses { get; set; } = new List<AnalyseCV>();
}