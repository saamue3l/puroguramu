using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.App.Pages.Students.EntryModels;
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
    private readonly IProgresRepository _progresRepository;
    private readonly UserManager<PuroUser> _userManager;
    private readonly IAssessExercise _assessor;

    [BindProperty]
    public Lesson Lesson { get; set; } = null!;

    [BindProperty]
    public Exercise ExerciseObject { get; set; } = null!;

    public Domains.Models.Progress Progress { get; set; } = null!;

    public string OriginalStub { get; set; } = null!;

    public string Stub { get; set; } = null!;

    public string Solution { get; set; } = null!;

    [BindProperty]
    public string UserCode { get; set; } = null!;

    public Exercise NextExercise { get; set; } = null!;

    public DoExercise(ILessonRepository lessonRepository, IExerciseRepository exerciseRepository, IProgresRepository progresRepository, UserManager<PuroUser> userManager, IAssessExercise assessor)
    {
        _lessonRepository = lessonRepository;
        _exerciseRepository = exerciseRepository;
        _progresRepository = progresRepository;
        _userManager = userManager;
        _assessor = assessor;
    }

    public IActionResult OnGet(int lessonId, int exerciseId)
    {
        Lesson = _lessonRepository.GetLesson(lessonId);

        if (Lesson.IDStatut == 1)
        {
            return RedirectToPage("/Students/Index");
        }

        ExerciseObject = _exerciseRepository.GetExercise(exerciseId);

        if (ExerciseObject.IDLecon != lessonId || ExerciseObject.IDStatut == 1)
        {
            return RedirectToPage("/Students/Index");
        }

        OriginalStub = ExerciseObject.Stub;
        Solution = ExerciseObject.Solution;

        var userId = _userManager.GetUserId(User);
        var progress = _progresRepository.GetProgres(exerciseId, userId);

        if (progress == null)
        {
            progress = new Domains.Models.Progress()
            {
                IDExercice = exerciseId,
                IDUtilisateur = userId,
                IDStatut = 1,
                CodeDerniereTentative = ExerciseObject.Stub,
                DateDerniereTentative = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            };
            _progresRepository.CreateProgres(progress);
        }

        Progress = progress;

        if (!string.IsNullOrEmpty(progress.CodeDerniereTentative))
        {
            Stub = progress.CodeDerniereTentative;
        }
        else
        {
            Stub = ExerciseObject.Stub;
        }

        NextExercise = _lessonRepository.GetNextExercise(ExerciseObject.IDLecon, ExerciseObject.IDExercice);

        return Page();
    }

    public async Task<IActionResult> OnPostUpdateProgress([FromBody] UpdateProgressInputModel inputModel)
    {
        var exerciseId = inputModel.ExerciseId;
        var userCode = inputModel.UserCode;

        var exerciseResult = await _assessor.Assess(exerciseId, userCode);
        var testResults = exerciseResult.TestResults;
        var userId = _userManager.GetUserId(User);
        var progress = _progresRepository.GetProgres(exerciseId, userId);
        var status = string.Empty;
        if (progress.IDStatut == 1 || progress.IDStatut == 2)
        {
            _progresRepository.UpdateProgresAttempt(exerciseId, userId, userCode);
            if (exerciseResult.Status == ExerciseStatus.Passed)
            {
                _progresRepository.UpdateProgresStatus(exerciseId, userId, 3);
                status = "Passed";
            }
            else
            {
                _progresRepository.UpdateProgresStatus(exerciseId, userId, 2);
                status = "Failed";
            }
        }

        var results = testResults.Select(tr => new { tr.Label, Status = tr.Status.ToString(), Error = tr.ErrorMessage }).ToList();

        return new JsonResult(new { Status = status, Results = results });
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnPostReset(int lessonId, int exerciseId)
    {
        var userId = _userManager.GetUserId(User);
        var progress = _progresRepository.GetProgres(exerciseId, userId);
        if (progress != null)
        {
            var exercise = _exerciseRepository.GetExercise(exerciseId);
            var originalStub = exercise.Stub;
            _progresRepository.UpdateProgresAttempt(exerciseId, userId, originalStub);
        }

        return RedirectToPage(new { lessonId, exerciseId });
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnPostAbandon(int lessonId, int exerciseId)
    {
        var userId = _userManager.GetUserId(User);
        var progress = _progresRepository.GetProgres(exerciseId, userId);
        if (progress != null)
        {
            _progresRepository.UpdateProgresStatus(exerciseId, userId, 4);
        }

        TempData["NotificationType"] = "error";
        TempData["NotificationMessage"] = "Exercice abandonné.";

        return RedirectToPage(new { lessonId, exerciseId });
    }
}
