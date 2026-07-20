using CVApplication.Models;

namespace CVApplication.Services.Interfaces;

public interface IRecommandationService
{
    List<Recommandation> GenererConseils(AnalyseResultat analyse);

    List<Recommandation> PrioriserConseils(List<Recommandation> conseils);

    Recommandation? GetRecommandationById(int id);
}