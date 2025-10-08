using System.ComponentModel.DataAnnotations;

namespace Puroguramu.Domains.Models;

public class Exercise
{
    public int IDExercice { get; set; }

    [Required]
    public string Titre { get; set; }
    public string Enonce { get; set; }
    public string Modele { get; set; }
    public string Solution { get; set; }

    public int Position { get; set; }
    public int IDLecon { get; set; }
    public int IDDifficulte { get; set; }

    public int IDStatut { get; set; }

    public Lesson Lesson { get; set; }
    public Difficulty Difficulty { get; set; }

    public Statut Statut { get; set; }

    public string Stub => @"public class Exercice
{
}
";

    public string InjectIntoTemplate(string code)
        => Modele.Replace("", code);
}
