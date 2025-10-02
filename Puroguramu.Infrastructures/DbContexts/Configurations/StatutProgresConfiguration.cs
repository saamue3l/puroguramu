using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Puroguramu.Domains.Models;

namespace Puroguramu.Infrastructures.DbContexts.Configurations;

public class StatutProgresConfiguration : IEntityTypeConfiguration<ProgressStatus>
{
    public void Configure(EntityTypeBuilder<ProgressStatus> builder)
    {
        builder.HasKey(sp => sp.IDStatut);

        builder.Property(sp => sp.Nom)
            .IsRequired()
            .HasMaxLength(255);

        SeedData(builder);
    }

    private void SeedData(EntityTypeBuilder<ProgressStatus> builder)
    {
        builder.HasData(
            new ProgressStatus { IDStatut = 1, Nom = "À faire" },
            new ProgressStatus { IDStatut = 2, Nom = "En cours" },
            new ProgressStatus { IDStatut = 3, Nom = "Résolu" },
            new ProgressStatus { IDStatut = 4, Nom = "Abandonné" });
    }
}
