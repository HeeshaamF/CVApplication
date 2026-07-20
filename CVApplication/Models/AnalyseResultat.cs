namespace CVApplication.Models;

public class AnalyseResultat
{
    public bool HasProfil { get; set; }

    public bool HasFormation { get; set; }

    public bool HasExperience { get; set; }

    public bool HasCompetences { get; set; }

    public bool HasLangues { get; set; }
    
    public bool HasContact { get; set; }
    
    public List<string> Competences { get; set; } = new List<string>();
    
    public List<string> Langues { get; set; } = new List<string>();
}