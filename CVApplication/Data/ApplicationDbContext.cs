using CVApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CVApplication.Data;

public class ApplicationDbContext : IdentityDbContext<User> {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
    }

    public DbSet<CV> CVs { get; set; }
    public DbSet<AnalyseCV> AnalyseCVs { get; set; }
    public DbSet<Recommandation> Recommandations { get; set; }
    public DbSet<OffreEmploi> OffresEmploi { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CV>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId);

        modelBuilder.Entity<AnalyseCV>()
            .HasOne(a => a.CV)
            .WithMany(c => c.AnalysesCV)
            .HasForeignKey(a => a.CVId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AnalyseCV>()
            .HasOne(a => a.OffreEmploi)
            .WithMany(o => o.Analyses)
            .HasForeignKey(a => a.OffreEmploiId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Recommandation>()
            .HasOne(r => r.AnalyseCV)
            .WithMany(a => a.Recommandations)
            .HasForeignKey(r => r.AnalyseCVId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CV>()
            .Property(c => c.Nom)
            .HasMaxLength(255);
    }
}