using CVApplication.Models;

namespace CVApplication.Services.Interfaces;

public interface IMatchingService
{
    int CalculerMatching(string cvText, OffreEmploi offre);

    MatchingResult ComparerAvecOffre(string cvText, OffreEmploi offre);
}