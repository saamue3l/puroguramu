using Microsoft.Build.Framework;

namespace Puroguramu.App.Pages.Teachers.EntryModels;

public class CreateLessonInputModel
{
    [Required]
    public string NewLessonName { get; set; }
}
