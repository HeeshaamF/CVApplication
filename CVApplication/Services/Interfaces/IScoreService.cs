using CVApplication.Models;

namespace CVApplication.Services.Interfaces;

public interface IScoreService
{
    int CalculerScore(AnalyseResultat analyse, string texte);

    int EvaluerStructure(AnalyseResultat analyse);

    int EvaluerCompetences(AnalyseResultat analyse);

    int EvaluerLisibilite(string texte);
}