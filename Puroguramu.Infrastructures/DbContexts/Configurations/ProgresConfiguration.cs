using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Puroguramu.Domains;

namespace Puroguramu.Infrastructures.DbContexts.Configurations;

public class ProgresConfiguration : IEntityTypeConfiguration<Progress>
{
    public void Configure(EntityTypeBuilder<Progress> builder)
    {
        builder.HasKey(p => p.IDProgres);

        builder.Property(p => p.CodeDerniereTentative);

        builder.Property(p => p.DateDerniereTentative)
            .HasMaxLength(255);

        builder.HasOne(p => p.Utilisateur)
            .WithMany()
            .HasForeignKey(p => p.IDUtilisateur)
            .IsRequired();

        builder.HasOne(p => p.ProgressStatus)
            .WithMany()
            .HasForeignKey(p => p.IDStatut)
            .IsRequired();

        builder.HasOne(p => p.Exercice)
            .WithMany()
            .HasForeignKey(p => p.IDExercice)
            .IsRequired();
    }

}
