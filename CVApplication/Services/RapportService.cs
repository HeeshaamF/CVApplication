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

                // Header
                page.Header().Text($"Rapport d'analyse - {rapport.NomCV}")
                    .FontSize(22).Bold().FontColor(Colors.Blue.Medium);

                page.Content().Column(col =>
                {
                    col.Spacing(20);

                    // Score global avec couleur dynamique
                    col.Item().Text($"Score global : {rapport.ScoreGlobal}%")
                        .FontSize(20).Bold()
                        .FontColor(rapport.ScoreGlobal >= 80 ? Colors.Green.Darken2 :
                                   rapport.ScoreGlobal >= 50 ? Colors.Orange.Darken2 :
                                   Colors.Red.Darken2);

                    // Fonction utilitaire pour afficher une barre de progression
                    void AddProgress(string label, double value)
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeColumn().Text(label).FontSize(12).Bold();
                            row.RelativeColumn().Stack(stack =>
                            {
                                stack.Item().Border(1).Background(Colors.Grey.Lighten2)
                                    .Height(15f) // ✅ float
                                    .Row(inner =>
                                    {
                                        inner.RelativeColumn((float)value) // ✅ cast en float
                                            .Background(
                                                value >= 80 ? Colors.Green.Darken2 :
                                                value >= 50 ? Colors.Orange.Darken2 :
                                                Colors.Red.Darken2);
                                    });
                                stack.Item().Text($"{value}%").AlignRight().FontSize(10);
                            });
                        });
                    }

                    // Sous-scores
                    AddProgress("Structure", rapport.ScoreStructure);
                    AddProgress("Compétences", rapport.ScoreCompetences);
                    AddProgress("Lisibilité", rapport.ScoreLisibilite);

                    // Recommandations
                    col.Item().Text("Recommandations :")
                        .FontSize(16).Bold().FontColor(Colors.Blue.Medium);

                    foreach (var rec in rapport.Recommandations)
                    {
                        string badge = rec.Priorite switch
                        {
                            1 => "⚠️ Critique",
                            2 => "⭐ Important",
                            3 => "ℹ️ Optionnel",
                            _ => ""
                        };

                        col.Item().Text($"- {rec.Message} ({badge})").FontSize(12);
                    }
                });

                // Footer
                page.Footer().AlignCenter()
                    .Text($"CVApplication - Rapport généré automatiquement le {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .FontSize(10).FontColor(Colors.Grey.Darken1);
            });
        });

        return document.GeneratePdf();
    }
}
