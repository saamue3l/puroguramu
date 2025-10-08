using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.App.Constants;
using Puroguramu.App.ViewModels;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.App.Pages.Students;

[Authorize(Roles = "Etudiant")]
public class Index : PageModel
{
    private readonly ILessonRepository _repository;
    private readonly UserManager<PuroUser> _userManager;

    public StudentsIndexViewModel ViewModel { get; set; } = null!;

    public Index(ILessonRepository repository, UserManager<PuroUser> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = _userManager.GetUserId(User);
        var publishedLessons = (await _repository.GetAllLessonsAsync())
            .Where(lesson => lesson.IDStatut == (int)EntityStatusEnum.Published);

        var lessonProgressItems = new List<LessonProgressItem>();

        foreach (var lesson in publishedLessons)
        {
            var (totalExercises, completedExercises) = await _repository.GetLessonProgressAsync(lesson.IDLecon, userId);
            lessonProgressItems.Add(new LessonProgressItem
            {
                Lesson = lesson,
                TotalExercises = totalExercises,
                CompletedExercises = completedExercises
            });
        }

        ViewModel = new StudentsIndexViewModel
        {
            Lessons = lessonProgressItems,
            NextExercise = await _repository.GetNextUncompletedExerciseAsync(userId),
            LastAttemptedExercise = await _repository.GetLastAttemptedExerciseAsync(userId)
        };

        return Page();
    }

    public async Task<IActionResult> OnGetNextExerciseAsync()
    {
        var userId = _userManager.GetUserId(User);
        var nextExercise = await _repository.GetNextUncompletedExerciseAsync(userId);

        if (nextExercise != null)
        {
            return RedirectToPage("/Students/DoExercise", new { lessonId = nextExercise.IDLecon, exerciseId = nextExercise.IDExercice });
        }

        return Page();
    }

    public async Task<IActionResult> OnGetContinueExerciseAsync()
    {
        var userId = _userManager.GetUserId(User);
        var continueExercise = await _repository.GetLastAttemptedExerciseAsync(userId);

        if (continueExercise != null)
        {
            return RedirectToPage("/Students/DoExercise", new { lessonId = continueExercise.IDLecon, exerciseId = continueExercise.IDExercice });
        }

        return Page();
    }
}
