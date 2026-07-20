using CVApplication.Models;

namespace CVApplication.Services.Interfaces;

public interface IOffreEmploiService
{
    Task<IEnumerable<OffreEmploi>> VoirListe();

    Task<OffreEmploi?> GetById(int id);

    Task AjouterOffre(OffreEmploi offre);

    Task ModifierOffre(OffreEmploi offre);

    Task SupprimerOffre(int id);
}