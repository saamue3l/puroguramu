using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;

namespace Puroguramu.App.Pages
{
    public class EditLesson : PageModel
    {
        private readonly ILessonRepository _repository;

        public Lesson? Lesson { get; set; }

        public IEnumerable<Exercise>? Exercises { get; set; }

        public EditLesson(ILessonRepository repository)
        {
            _repository = repository;
        }

        public IActionResult OnGet(int lessonId)
        {
            Lesson = _repository.GetLesson(lessonId);
            Exercises = _repository.GetExercisesForLesson(lessonId);
            return Page();
        }
    }
}
