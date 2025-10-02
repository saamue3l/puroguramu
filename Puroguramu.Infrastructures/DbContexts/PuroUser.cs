using Microsoft.AspNetCore.Identity;
using Puroguramu.Domains.Models;

namespace Puroguramu.Infrastructures.DbContexts;

public class PuroUser : IdentityUser
{
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public string? Image { get; set; }
    public int IDGroupe { get; set; }
    public Group? Groupe { get; set; }
}
