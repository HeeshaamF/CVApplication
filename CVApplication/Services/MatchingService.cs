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

        // Découpage en tokens (mots entiers)
        var tokens = Regex.Split(texteCV.ToLowerInvariant(), @"[^a-z0-9#+]+")
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(Normalize)
            .ToList();

        int matchedCount = 0;

        foreach (var skill in competences)
        {
            // Matching strict ou tolérant
            if (tokens.Any(t => t == skill) || Levenshtein(skill, tokens))
            {
                matchedCount++;
            }
        }

        // Score binaire simple : ratio compétences trouvées / attendues
        return (int)Math.Round((double)matchedCount / competences.Count * 100);
    }
    
    // Matching tolérant : faute légère (Levenshtein)
    private bool Levenshtein(string skill, List<string> tokens)
    {
        int threshold = skill.Length <= 4 ? 1 : 2;
        return tokens.Any(t => ComputeDistance(skill, t) <= threshold);
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
