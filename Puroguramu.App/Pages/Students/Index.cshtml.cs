using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.App.Pages.Students;

[Authorize(Roles = "Etudiant")]
public class Index : PageModel
{
    private readonly ILessonRepository _repository;
    private readonly UserManager<PuroUser> _userManager;

    public List<(Lesson Lesson, int TotalExercises, int CompletedExercises)> Lessons { get; set; }

    public Exercise NextExercise { get; set; }

    public Exercise LastAttemptedExercise { get; set; }

    public Index(ILessonRepository repository, UserManager<PuroUser> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = _userManager.GetUserId(User);
        var lessons = _repository.GetAllLessons().Where(lesson => lesson.IDStatut == 2);
        Lessons = new List<(Lesson, int, int)>();

        foreach (var lesson in lessons)
        {
            var (totalExercises, completedExercises) = _repository.GetLessonProgress(lesson.IDLecon, userId);
            Lessons.Add((lesson, totalExercises, completedExercises));
        }

        NextExercise = _repository.GetNextUncompletedExercise(userId);

        LastAttemptedExercise = _repository.GetLastAttemptedExercise(userId);

        return Page();
    }

    public async Task<IActionResult> OnGetNextExerciseAsync()
    {
        var userId = _userManager.GetUserId(User);
        var nextExercise = _repository.GetNextUncompletedExercise(userId);

        if (nextExercise != null)
        {
            return RedirectToPage("/Students/DoExercise", new { lessonId = nextExercise.IDLecon, exerciseId = nextExercise.IDExercice });
        }

        return Page();
    }

    public async Task<IActionResult> OnGetContinueExerciseAsync()
    {
        var userId = _userManager.GetUserId(User);
        var continueExercise = _repository.GetLastAttemptedExercise(userId);

        if (continueExercise != null)
        {
            return RedirectToPage("/Students/DoExercise", new { lessonId = continueExercise.IDLecon, exerciseId = continueExercise.IDExercice });
        }

        return Page();
    }
}
