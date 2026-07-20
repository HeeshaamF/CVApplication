using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVApplication.Models;

public class Recommandation {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int AnalyseCVId { get; set; }
    
    public string Message { get; set; } = string.Empty;
    
    public int Priorite { get; set; }

    public virtual AnalyseCV AnalyseCV { get; set; } = null;
}