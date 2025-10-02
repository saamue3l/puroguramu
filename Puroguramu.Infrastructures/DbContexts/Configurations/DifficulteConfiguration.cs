using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Puroguramu.Domains.Models;

namespace Puroguramu.Infrastructures.DbContexts.Configurations;

public class DifficulteConfiguration : IEntityTypeConfiguration<Difficulty>
{
    public void Configure(EntityTypeBuilder<Difficulty> builder)
    {
        builder.HasKey(d => d.IdDifficulte);

        builder.Property(d => d.Libelle)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(d => d.Libelle)
            .IsUnique();

        SeedData(builder);
    }

    private void SeedData(EntityTypeBuilder<Difficulty> builder)
    {
        builder.HasData(
            new Difficulty { IdDifficulte = 1, Libelle = "Facile" },
            new Difficulty { IdDifficulte = 2, Libelle = "Moyen" },
            new Difficulty { IdDifficulte = 3, Libelle = "Difficile" });
    }
}
