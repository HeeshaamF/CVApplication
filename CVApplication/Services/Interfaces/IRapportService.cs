using CVApplication.Models;

namespace CVApplication.Services.Interfaces;

public interface IRapportService
{
    Rapport GenererRapport(AnalyseCV analyse, IEnumerable<Recommandation> conseils);

    byte[] ExporterPDF(Rapport rapport);
}