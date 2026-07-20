using CVApplication.Models;

namespace CVApplication.Services.Interfaces;

public interface IAnalyseService
{
    string ExtraireTexte(string chemin);

    AnalyseResultat DetecterRubriques(string texte);

    AnalyseCV AnalyserCV(CV cv, OffreEmploi? offre);
    
    List<AnalyseCV> GetAnalysesByCVId(int cvId);

    AnalyseCV? GetAnalyseById(int id);
    
    Task SaveAnalyse(AnalyseCV analyse);
    
    CV? GetCVById(int id);
    
    Task<List<AnalyseCV>> GetHistoriqueAnalyses(string userId);
}