using Microsoft.Build.Framework;

namespace Puroguramu.App.Pages.Teachers.EntryModels;

public class CreateExerciseInputModel
{
    [Required]
    public string NewExerciseName { get; set; }
}
