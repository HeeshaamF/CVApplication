using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVApplication.Models;

public class CV {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}
    
    public string UserId { get; set; }
    
    public string Nom { get; set; } = string.Empty;
    
    public string Chemin { get; set; } = string.Empty;
    
    public DateTime DateUpload { get; set; } =  DateTime.Now;

    public double ScoreGlobal { get; set; }
    
    public User User { get; set; }
    
    public ICollection<AnalyseCV> AnalysesCV { get; set; } = new List<AnalyseCV>();
}