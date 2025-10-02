using System.ComponentModel.DataAnnotations;

namespace Puroguramu.Domains.Models;

public class Lesson
{
    public int IDLecon { get; set; }

    [Required]
    public string Intitule { get; set; }
    public string Description { get; set; }

    public int Position { get; set; }
    public int IDStatut { get; set; }

    public Statut Statut { get; set; }
}
