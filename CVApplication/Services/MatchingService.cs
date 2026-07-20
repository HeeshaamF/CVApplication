using System.Text.RegularExpressions;
using CVApplication.Models;
using CVApplication.Services.Interfaces;

namespace CVApplication.Services;

public class MatchingService : IMatchingService
{
    public int CalculerMatching(string texteCV, OffreEmploi offre)
    {
        var competences = offre.CompetencesAttendues
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(Normalize)
            .ToList();

        if (!competences.Any())
            return 0;

        var tokens = Regex.Split(texteCV.ToLowerInvariant(), @"[^a-z0-9#+]+")
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(Normalize)
            .ToList();

        double totalScore = 0;
        int matchedCount = 0;

        foreach (var skill in competences)
        {
            double score = 0;

            if (tokens.Any(t => t == skill))
            {
                score = 1.0;
                matchedCount++;
            }
            else if (tokens.Any(t => t.Contains(skill) || skill.Contains(t)))
            {
                score = 0.7;
                matchedCount++;
            }
            else if (Levenshtein(skill, tokens))
            {
                score = 0.4;
                matchedCount++;
            }

            totalScore += score;
        }

        // Si toutes les compétences attendues sont trouvées (exactes, partielles ou proches)
        if (matchedCount == competences.Count)
            return 100;

        return (int)Math.Round(totalScore / competences.Count * 100);
    }

    
    // Calcul de la distance d'édition/Levenshtein (méthodes Levenshtein et ComputeDistance)
    // Pour identifier les éventuelles fautes de frappe et suggérer les mots les plus proches
    private bool Levenshtein(string skill, List<string> tokens)
    {
        return tokens.Any(t => ComputeDistance(skill, t) <= 2);
    }
    
    private int ComputeDistance(string a, string b)
    {
        var dp = new int[a.Length + 1, b.Length + 1];

        for (int i = 0; i <= a.Length; i++)
        for (int j = 0; j <= b.Length; j++)
        {
            if (i == 0) dp[i, j] = j;
            else if (j == 0) dp[i, j] = i;
            else if (a[i - 1] == b[j - 1])
                dp[i, j] = dp[i - 1, j - 1];
            else
                dp[i, j] = 1 + Math.Min(dp[i - 1, j - 1], Math.Min(dp[i - 1, j], dp[i, j - 1]));
        }

        return dp[a.Length, b.Length];
    }

    private string Normalize(string input)
    {
        return input.Trim()
            .ToLowerInvariant()
            .Replace("c#", "csharp")
            .Replace("c++", "cpp");
    }

    public MatchingResult ComparerAvecOffre(string texteCV, OffreEmploi offre)
    {
        return new MatchingResult
        {
            Score = CalculerMatching(texteCV, offre)
        };
    }
}