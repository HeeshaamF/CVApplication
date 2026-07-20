namespace CVApplication.Models;

public class AnalyseCV {
    public int Id { get; set; }

    public int CVId { get; set; }

    public int? OffreEmploiId { get; set; }

    public double ScoreStructure { get; set; }
    
    public double ScoreCompetences { get; set; }
    
    public double ScoreLisibilite { get; set; }
    
    public double ScoreMatching { get; set; }

    public CV CV { get; set; }

    public OffreEmploi? OffreEmploi { get; set; }

    public ICollection<Recommandation> Recommandations { get; set; } = new List<Recommandation>();
}