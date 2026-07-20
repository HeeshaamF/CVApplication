using CVApplication.Data;
using CVApplication.Models;
using CVApplication.Services.Interfaces;
using CVApplication.Helpers;

namespace CVApplication.Services;

public class RecommandationService : IRecommandationService
{
    private readonly ApplicationDbContext _context;

    public RecommandationService(ApplicationDbContext context)
    {
        _context = context;
    }
    
   public List<Recommandation> GenererConseils(AnalyseResultat analyse)
    {
        var conseils = new List<Recommandation>();

        // Rubriques
        if (!analyse.HasProfil)
            conseils.Add(new Recommandation { Message = "Ajoutez un résumé de profil.", Priorite = 1 });

        if (!analyse.HasFormation)
            conseils.Add(new Recommandation { Message = "Ajoutez vos diplômes et formations.", Priorite = 1 });

        if (!analyse.HasExperience)
            conseils.Add(new Recommandation { Message = "Ajoutez vos expériences professionnelles.", Priorite = 1 });

        if (!analyse.HasContact)
            conseils.Add(new Recommandation { Message = "Ajoutez vos informations de contact.", Priorite = 1 });

        // Compétences
        if (!analyse.HasCompetences || !analyse.Competences.Any())
        {
            conseils.Add(new Recommandation { Message = "Ajoutez vos compétences techniques et personnelles.", Priorite = 2 });
        }

        // Langues
        if (!analyse.HasLangues || !analyse.Langues.Any())
        {
            conseils.Add(new Recommandation { Message = "Ajoutez vos langues maîtrisées.", Priorite = 3 });
        }
        else
        {
            // Vérifier si les langues ont un niveau
            bool hasNiveau = analyse.Langues.Any(l => SkillKeywords.NiveauxLangue.Any(n => l.ToLower().Contains(n)));

            if (!hasNiveau)
            {
                conseils.Add(new Recommandation { Message = "Précisez votre niveau de langue (ex. B1, C1).", Priorite = 3 });
            }
        }
        return conseils;
    }

    public List<Recommandation> PrioriserConseils(List<Recommandation> conseils)
    {
        return conseils.OrderBy(c => c.Priorite).ToList();
    }

    public Recommandation? GetRecommandationById(int id)
    {
        return _context.Recommandations.FirstOrDefault(r => r.Id == id);
    }
}
