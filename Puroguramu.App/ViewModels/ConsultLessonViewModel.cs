using Puroguramu.Domains.Models;

namespace Puroguramu.App.ViewModels;

public class ConsultLessonViewModel
{
    public int LessonId { get; set; }
    public string LessonTitle { get; set; } = string.Empty;
    public string LessonDescription { get; set; } = string.Empty;

    public List<ExerciseProgressItem> Exercises { get; set; } = new();

    public bool HasExercises => Exercises.Any();

    public List<ExerciseProgressItem> FirstColumnExercises
    {
        get
        {
            var halfCount = (int)Math.Ceiling(Exercises.Count / 2.0);
            return Exercises.Take(halfCount).ToList();
        }
    }

    public List<ExerciseProgressItem> SecondColumnExercises
    {
        get
        {
            var halfCount = (int)Math.Ceiling(Exercises.Count / 2.0);
            return Exercises.Skip(halfCount).ToList();
        }
    }
}

public class ExerciseProgressItem
{
    public int LessonId { get; set; }
    public Exercise Exercise { get; set; } = null!;
    public string ProgressStatusName { get; set; } = string.Empty;
}
