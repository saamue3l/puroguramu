using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Puroguramu.Domains.Models;

namespace Puroguramu.Infrastructures.DbContexts.Configurations;

public class StatutConfiguration : IEntityTypeConfiguration<Statut>
{
    public void Configure(EntityTypeBuilder<Statut> builder)
    {
        builder.HasKey(sl => sl.IDStatut);

        builder.Property(sl => sl.Nom)
            .IsRequired()
            .HasMaxLength(255);

        SeedData(builder);
    }

    private void SeedData(EntityTypeBuilder<Statut> builder)
    {
        builder.HasData(
            new Statut { IDStatut = 1, Nom = "Masqué" },
            new Statut { IDStatut = 2, Nom = "Publié" });
    }
}
