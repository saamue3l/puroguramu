using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Puroguramu.Domains.Models;

namespace Puroguramu.Infrastructures.DbContexts.Configurations;

public class LeconConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(l => l.IDLecon);

        builder.Property(l => l.Intitule)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(l => l.Description).IsRequired(false);

        builder.Property(l => l.Position)
            .IsRequired();

        builder.HasOne(l => l.Statut)
            .WithMany()
            .HasForeignKey(l => l.IDStatut);

        builder.HasIndex(l => l.Intitule)
            .IsUnique();

        SeedData(builder);
    }

    private void SeedData(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasData(
            new Lesson
            {
                IDLecon = 1,
                Intitule = "Introduction à C#",
                Position = 1,
                IDStatut = 2,
                Description = "Cette leçon couvre les bases de C#, incluant la syntaxe de base, les types de données et les méthodes simples."
            },
            new Lesson
            {
                IDLecon = 2,
                Intitule = "Structures de contrôle en C#",
                Position = 2,
                IDStatut = 2,
                Description = "Cette leçon explore les structures de contrôle en C#, telles que les boucles et les instructions conditionnelles."
            },
            new Lesson
            {
                IDLecon = 3,
                Intitule = "Programmation orientée objet en C#",
                Position = 3,
                IDStatut = 2,
                Description = "Cette leçon couvre les concepts de base de la programmation orientée objet en C#."
            },
            new Lesson
            {
                IDLecon = 4,
                Intitule = "Gestion des exceptions en C#",
                Position = 4,
                IDStatut = 2,
                Description = "Cette leçon couvre la gestion des exceptions en C#."
            },
            new Lesson
            {
                IDLecon = 5,
                Intitule = "Programmation asynchrone en C#",
                Position = 5,
                IDStatut = 2,
                Description = "Cette leçon couvre la programmation asynchrone en C#."
            }
        );
    }
}
