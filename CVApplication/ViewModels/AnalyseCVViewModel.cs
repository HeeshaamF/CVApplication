using CVApplication.Models;

namespace CVApplication.ViewModels;

public class AnalyseCVViewModel
{
    public CV CV { get; set; }

    public List<AnalyseCV> Analyses { get; set; }
}