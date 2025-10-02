namespace Puroguramu.App.Pages.Teachers.EntryModels;

public class EditExerciceSaveChangesInputModel
{
    public int LessonId { get; set; }

    public int ExerciseId { get; set; }

    public string Titre { get; set; }

    public string Enonce { get; set; }

    public int IDDifficulte { get; set; }

    public string Modele { get; set; }

    public string Solution { get; set; }
}
