using CVApplication.Models;
using CVApplication.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;

public class RapportService : IRapportService
{
    public Rapport GenererRapport(AnalyseCV analyse, IEnumerable<Recommandation> conseils)
    {
        return new Rapport
        {
            NomCV = analyse.CV.Nom,
            ScoreGlobal = (int)analyse.CV.ScoreGlobal,
            ScoreStructure = analyse.ScoreStructure,
            ScoreCompetences = analyse.ScoreCompetences,
            ScoreLisibilite = analyse.ScoreLisibilite,
            Resume = $"Structure={analyse.ScoreStructure}%, Compétences={analyse.ScoreCompetences}%, Lisibilité={analyse.ScoreLisibilite}%",
            Recommandations = conseils.ToList()
        };
    }


    public byte[] ExporterPDF(Rapport rapport)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);

                page.Header().Text($"Rapport d'analyse - {rapport.NomCV}").FontSize(22).Bold().FontColor(Colors.Blue.Medium);

                page.Content().Column(col =>
                {
                    col.Spacing(15);

                    // Score global
                    col.Item().Text($"Score global : {rapport.ScoreGlobal}%").FontSize(18).Bold().FontColor(Colors.Green.Darken2);

                    // Résumé
                    col.Item().Text($"Résumé des scores : ").FontSize(14);

                    // Tableau des sous-scores
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Critère").Bold();
                            header.Cell().Text("Score").Bold();
                        });

                        table.Cell().Text("Structure");
                        table.Cell().Text($"{rapport.Resume.Split(',')[0].Split('=')[1]}");

                        table.Cell().Text("Compétences");
                        table.Cell().Text($"{rapport.Resume.Split(',')[1].Split('=')[1]}");

                        table.Cell().Text("Lisibilité");
                        table.Cell().Text($"{rapport.Resume.Split(',')[2].Split('=')[1]}");
                        
                    });

                    // Recommandations
                    col.Item().Text("Recommandations :").FontSize(16).Bold().FontColor(Colors.Red.Medium);

                    foreach (var rec in rapport.Recommandations)
                    {
                        string badge = rec.Priorite switch
                        {
                            1 => "Critique",
                            2 => "Important",
                            3 => "Optionnel",
                            _ => ""
                        };

                        col.Item().Text($"- {rec.Message} ({badge})").FontSize(12);
                    }
                });

                page.Footer().AlignCenter().Text("CVApplication - Rapport généré automatiquement").FontSize(10).FontColor(Colors.Grey.Darken1);
            });
        });

        return document.GeneratePdf();
    }
}
