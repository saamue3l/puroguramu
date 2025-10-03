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

        public async Task<IActionResult> OnGetAsync(int lessonId)
        {
            Lesson = await _repository.GetLessonAsync(lessonId);
            Exercises = await _repository.GetExercisesForLessonAsync(lessonId);
            return Page();
        }
    }
}
