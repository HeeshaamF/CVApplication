using System.Text.RegularExpressions;

namespace CVApplication.Helpers
{
    public static class RubriqueKeywords
    {
        // Profil
        public static readonly Regex ProfilRegex = new(@"profil|profile|étudiant|recherche|stage|candidat", RegexOptions.IgnoreCase);

        // Formation
        public static readonly Regex FormationRegex = new(@"formation|f\s*o\s*r\s*m\s*a\s*t\s*i\s*o\s*n|education|diplome|parcours", RegexOptions.IgnoreCase);

        // Expérience
        public static readonly Regex ExperienceRegex = new(@"expérience|experience|e\s*x\s*p\s*é\s*r\s*i\s*e\s*n\s*c\s*e|stage|projet", RegexOptions.IgnoreCase);

        // Compétences
        public static readonly Regex CompetencesRegex = new(@"compétences|skills|c\s*o\s*m\s*p\s*é\s*t\s*e\s*n\s*c\s*e\s*s", RegexOptions.IgnoreCase);

        // Langues
        public static readonly Regex LanguesRegex = new(@"langues|languages|l\s*a\s*n\s*g\s*u\s*e\s*s", RegexOptions.IgnoreCase);

        // Contact
        public static readonly Regex ContactRegex = new(@"contact|c\s*o\s*n\s*t\s*a\s*c\s*t|email|téléphone", RegexOptions.IgnoreCase);
    }
}