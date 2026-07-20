namespace CVApplication.Models;

public class MatchingResult
{
    public int Score { get; set; }

    public List<string> CompetencesTrouvees { get; set; } = new();

    public List<string> CompetencesManquantes{ get; set; } = new();
}