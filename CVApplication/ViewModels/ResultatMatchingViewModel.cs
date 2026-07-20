using CVApplication.Models;
namespace CVApplication.ViewModels;

public class ResultatMatchingViewModel
{
    public CV CV { get; set; }

    public OffreEmploi Offre { get; set; }

    public double ScoreMatching { get; set; }
}