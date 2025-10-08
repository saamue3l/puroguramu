using Puroguramu.App.Constants;
using Puroguramu.Domains.Models;

namespace Puroguramu.App.ViewModels;

public class DoExerciseViewModel
{
    public int LessonId { get; set; }
    public string LessonTitle { get; set; } = string.Empty;

    public int ExerciseId { get; set; }
    public string ExerciseTitle { get; set; } = string.Empty;
    public string ExerciseStatement { get; set; } = string.Empty;
    public int DifficultyLevel { get; set; }

    public string OriginalStub { get; set; } = string.Empty;
    public string CurrentStub { get; set; } = string.Empty;
    public string Solution { get; set; } = string.Empty;

    public ProgressStatusEnum ProgressStatus { get; set; }

    public bool HasNextExercise => NextExercise != null;
    public Exercise? NextExercise { get; set; }

    public bool IsCompleted => ProgressStatus == ProgressStatusEnum.Completed;
    public bool IsAbandoned => ProgressStatus == ProgressStatusEnum.Abandoned;
    public bool CanShowSolution => IsCompleted || IsAbandoned;
    public bool CanSubmit => !IsCompleted && !IsAbandoned;
    public bool ShowTestResults => !CanShowSolution;

    public string StatusText => ProgressStatus switch
    {
        ProgressStatusEnum.Completed => "Réussi",
        ProgressStatusEnum.Abandoned => "Abandonné",
        _ => string.Empty
    };

    public string StatusColorClass => ProgressStatus switch
    {
        ProgressStatusEnum.Completed => "bg-custom-green",
        ProgressStatusEnum.Abandoned => "bg-custom-red",
        _ => string.Empty
    };
}
