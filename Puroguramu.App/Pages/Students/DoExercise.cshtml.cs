using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.App.Constants;
using Puroguramu.App.Pages.Students.EntryModels;
using Puroguramu.App.Services;
using Puroguramu.App.ViewModels;
using Puroguramu.Domains;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.App.Pages.Students;

[Authorize(Roles = "Etudiant")]
public class DoExercise : PageModel
{
    private readonly ILessonRepository _lessonRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IProgressService _progressService;
    private readonly UserManager<PuroUser> _userManager;
    private readonly IAssessExercise _assessor;

    public DoExerciseViewModel ViewModel { get; set; } = null!;

    public DoExercise(
        ILessonRepository lessonRepository,
        IExerciseRepository exerciseRepository,
        IProgressService progressService,
        UserManager<PuroUser> userManager,
        IAssessExercise assessor)
    {
        _lessonRepository = lessonRepository;
        _exerciseRepository = exerciseRepository;
        _progressService = progressService;
        _userManager = userManager;
        _assessor = assessor;
    }

    public async Task<IActionResult> OnGetAsync(int lessonId, int exerciseId)
    {
        var lesson = await _lessonRepository.GetLessonAsync(lessonId);

        if (lesson.IDStatut == (int)EntityStatusEnum.Draft)
        {
            return RedirectToPage("/Students/Index");
        }

        var exercise = await _exerciseRepository.GetExerciseAsync(exerciseId);

        if (exercise.IDLecon != lessonId || exercise.IDStatut == (int)EntityStatusEnum.Draft)
        {
            return RedirectToPage("/Students/Index");
        }

        var userId = _userManager.GetUserId(User);
        var progress = await _progressService.GetOrCreateProgressAsync(exerciseId, userId, exercise.Stub);

        var currentStub = !string.IsNullOrEmpty(progress.CodeDerniereTentative)
            ? progress.CodeDerniereTentative
            : exercise.Stub;

        var nextExercise = await _lessonRepository.GetNextExerciseAsync(exercise.IDLecon, exercise.IDExercice);

        ViewModel = new DoExerciseViewModel
        {
            LessonId = lesson.IDLecon,
            LessonTitle = lesson.Intitule,
            ExerciseId = exercise.IDExercice,
            ExerciseTitle = exercise.Titre,
            ExerciseStatement = exercise.Enonce,
            DifficultyLevel = exercise.IDDifficulte,
            OriginalStub = exercise.Stub,
            CurrentStub = currentStub,
            Solution = exercise.Solution,
            ProgressStatus = (ProgressStatusEnum)progress.IDStatut,
            NextExercise = nextExercise
        };

        return Page();
    }

    public async Task<IActionResult> OnPostUpdateProgress([FromBody] UpdateProgressInputModel inputModel)
    {
        var exerciseId = inputModel.ExerciseId;
        var userCode = inputModel.UserCode;
        var userId = _userManager.GetUserId(User);

        if (!await _progressService.CanUpdateProgressAsync(exerciseId, userId))
        {
            return new JsonResult(new { Status = "Error", Results = Array.Empty<object>() });
        }

        var exerciseResult = await _assessor.Assess(exerciseId, userCode);

        await _progressService.UpdateProgressAttemptAsync(exerciseId, userId, userCode);

        var status = string.Empty;
        if (exerciseResult.Status == ExerciseStatus.Passed)
        {
            await _progressService.UpdateProgressStatusAsync(exerciseId, userId, ProgressStatusEnum.Completed);
            status = "Passed";
        }
        else
        {
            await _progressService.UpdateProgressStatusAsync(exerciseId, userId, ProgressStatusEnum.InProgress);
            status = "Failed";
        }

        var results = exerciseResult.TestResults
            .Select(tr => new { tr.Label, Status = tr.Status.ToString(), Error = tr.ErrorMessage })
            .ToList();

        return new JsonResult(new { Status = status, Results = results });
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnPostResetAsync(int lessonId, int exerciseId)
    {
        var userId = _userManager.GetUserId(User);
        var exercise = await _exerciseRepository.GetExerciseAsync(exerciseId);
        await _progressService.UpdateProgressAttemptAsync(exerciseId, userId, exercise.Stub);

        return RedirectToPage(new { lessonId, exerciseId });
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnPostAbandonAsync(int lessonId, int exerciseId)
    {
        var userId = _userManager.GetUserId(User);
        await _progressService.UpdateProgressStatusAsync(exerciseId, userId, ProgressStatusEnum.Abandoned);

        TempData["NotificationType"] = "error";
        TempData["NotificationMessage"] = "Exercice abandonné.";

        return RedirectToPage(new { lessonId, exerciseId });
    }
}
