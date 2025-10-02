using Puroguramu.Domains.Models;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.Infrastructures.DbContexts;

public class Progress
{
    public int IDProgres { get; set; }
    public string DateDerniereTentative { get; set; }
    public string CodeDerniereTentative { get; set; }
    public string IDUtilisateur { get; set; }
    public int IDStatut { get; set; }
    public int IDExercice { get; set; }
    public PuroUser Utilisateur { get; set; }
    public Exercise Exercice { get; set; }
    public ProgressStatus ProgressStatus { get; set; }
}
