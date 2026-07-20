using CVApplication.Models;

namespace CVApplication.ViewModels;

public class ComparaisonCVViewModel
{
    public int CVId { get; set; }

    public int? OffreId { get; set; }

    public List<OffreEmploi> Offres { get; set; } = new();
}