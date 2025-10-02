using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Puroguramu.Domains.Models;

namespace Puroguramu.Infrastructures.DbContexts.Configurations;

public class GroupeConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(g => g.IDGroupe);

        builder.Property(g => g.Nom)
            .IsRequired()
            .HasMaxLength(4);

        SeedData(builder);
    }

    private void SeedData(EntityTypeBuilder<Group> builder)
    {
        builder.HasData(
            new Group { IDGroupe = 1, Nom = "2i1" },
            new Group { IDGroupe = 2, Nom = "2i2" },
            new Group { IDGroupe = 3, Nom = "2i3" },
            new Group { IDGroupe = 4, Nom = "2i4" });
    }
}
