using Puroguramu.Domains.Models;

namespace Puroguramu.App.ViewModels;

public class StudentsIndexViewModel
{
    public List<LessonProgressItem> Lessons { get; set; } = new();
    public Exercise? NextExercise { get; set; }
    public Exercise? LastAttemptedExercise { get; set; }

    public bool HasNextExercise => NextExercise != null;
    public bool HasLastAttemptedExercise => LastAttemptedExercise != null;
    public bool HasLessons => Lessons.Any();

    public List<LessonProgressItem> FirstColumnLessons
    {
        get
        {
            var halfCount = (int)Math.Ceiling(Lessons.Count / 2.0);
            return Lessons.Take(halfCount).ToList();
        }
    }

    public List<LessonProgressItem> SecondColumnLessons
    {
        get
        {
            var halfCount = (int)Math.Ceiling(Lessons.Count / 2.0);
            return Lessons.Skip(halfCount).ToList();
        }
    }
}

public class LessonProgressItem
{
    public Lesson Lesson { get; set; } = null!;
    public int TotalExercises { get; set; }
    public int CompletedExercises { get; set; }

    public int ProgressPercentage =>
        TotalExercises > 0 ? (int)(((double)CompletedExercises / TotalExercises) * 100) : 0;

    public string ProgressText => $"{CompletedExercises}/{TotalExercises}";
}
