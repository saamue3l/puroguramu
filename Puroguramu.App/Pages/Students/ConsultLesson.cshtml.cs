using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;
using Progress = Puroguramu.Domains.Models.Progress;

namespace Puroguramu.App.Pages.Students;

[Authorize(Roles = "Etudiant")]
public class ConsultLesson : PageModel
{
    private readonly ILessonRepository _repository;
    private readonly IProgresRepository _progresRepository;
    private readonly UserManager<PuroUser> _userManager;

    public Lesson Lesson { get; set; } = null!;

    public List<(Exercise Exercise, Progress Progress)> ExercisesWithProgress { get; set; } = null!;

    public ConsultLesson(ILessonRepository repository, IProgresRepository progresRepository, UserManager<PuroUser> userManager)
    {
        _repository = repository;
        _progresRepository = progresRepository;
        _userManager = userManager;
    }

    public IActionResult OnGet(int lessonId)
    {
        Lesson = _repository.GetLesson(lessonId);

        if (Lesson.IDStatut == 1)
        {
            return RedirectToPage("/Students/Index");
        }

        var exercises = _repository.GetExercisesForLessonStudent(lessonId);
        var userId = _userManager.GetUserId(User);
        ExercisesWithProgress = new List<(Exercise, Progress)>();

        foreach (var exercise in exercises)
        {
            var progress = _progresRepository.GetProgres(exercise.IDExercice, userId);
            if (progress == null)
            {
                progress = new Progress
                {
                    IDStatut = 0,
                    IDUtilisateur = userId,
                    IDExercice = exercise.IDExercice,
                    CodeDerniereTentative = string.Empty,
                    DateDerniereTentative = string.Empty,
                    ProgressStatus = new ProgressStatus { IDStatut = 1, Nom = "À faire" },
                };
            }

            if (progress.ProgressStatus.Nom != "Masqué")
            {
                ExercisesWithProgress.Add((exercise, progress));
            }
        }

        return Page();
    }
}
