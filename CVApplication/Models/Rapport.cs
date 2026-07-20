using CVApplication.Models;

public class Rapport
{
    public string NomCV { get; set; }
    public int ScoreGlobal { get; set; }
    public double ScoreStructure { get; set; }
    public double ScoreCompetences { get; set; }
    public double ScoreLisibilite { get; set; }
    public string Resume { get; set; }
    public List<Recommandation> Recommandations { get; set; } = new();
}