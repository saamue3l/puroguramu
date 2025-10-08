using Puroguramu.Domains.Models;

namespace Puroguramu.App.ViewModels;

public class TeachersIndexViewModel
{
    public List<TeacherLessonItem> Lessons { get; set; } = new();
    public string SortOrder { get; set; } = "position";

    public bool HasLessons => Lessons.Any();

    public List<TeacherLessonItem> FirstColumnLessons
    {
        get
        {
            var halfCount = (int)Math.Ceiling(Lessons.Count / 2.0);
            return Lessons.Take(halfCount).ToList();
        }
    }

    public List<TeacherLessonItem> SecondColumnLessons
    {
        get
        {
            var halfCount = (int)Math.Ceiling(Lessons.Count / 2.0);
            return Lessons.Skip(halfCount).ToList();
        }
    }
}

public class TeacherLessonItem
{
    public Lesson Lesson { get; set; } = null!;
    public int StudentsCompleted { get; set; }
    public int TotalStudents { get; set; }
    public int TotalLessons { get; set; }

    public string CompletionText => $"{StudentsCompleted}/{TotalStudents}";
}

public class CreateLessonInputModel
{
    public string NewLessonName { get; set; } = string.Empty;
}
