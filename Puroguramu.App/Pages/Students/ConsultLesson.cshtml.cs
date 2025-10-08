using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.App.Constants;
using Puroguramu.App.ViewModels;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.App.Pages.Students;

[Authorize(Roles = "Etudiant")]
public class ConsultLesson : PageModel
{
    private readonly ILessonRepository _repository;
    private readonly IProgresRepository _progresRepository;
    private readonly UserManager<PuroUser> _userManager;

    public ConsultLessonViewModel ViewModel { get; set; } = null!;

    public ConsultLesson(ILessonRepository repository, IProgresRepository progresRepository, UserManager<PuroUser> userManager)
    {
        _repository = repository;
        _progresRepository = progresRepository;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync(int lessonId)
    {
        var lesson = await _repository.GetLessonAsync(lessonId);

        if (lesson.IDStatut == (int)EntityStatusEnum.Draft)
        {
            return RedirectToPage("/Students/Index");
        }

        var exercises = await _repository.GetExercisesForLessonStudentAsync(lessonId);
        var userId = _userManager.GetUserId(User);
        var exerciseProgressItems = new List<ExerciseProgressItem>();

        foreach (var exercise in exercises)
        {
            var progress = await _progresRepository.GetProgresAsync(exercise.IDExercice, userId);

            var progressStatusName = progress?.ProgressStatus?.Nom ?? "À faire";

            if (progressStatusName != "Masqué")
            {
                exerciseProgressItems.Add(new ExerciseProgressItem
                {
                    LessonId = lessonId,
                    Exercise = exercise,
                    ProgressStatusName = progressStatusName
                });
            }
        }

        ViewModel = new ConsultLessonViewModel
        {
            LessonId = lesson.IDLecon,
            LessonTitle = lesson.Intitule,
            LessonDescription = lesson.Description,
            Exercises = exerciseProgressItems
        };

        return Page();
    }
}
