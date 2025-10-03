using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Puroguramu.App.Pages.Teachers.EntryModels;
using Puroguramu.Domains;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.App.Pages.Teachers;

[Authorize(Roles = "Enseignant")]
public class EditExercise : PageModel
{
    private readonly ILessonRepository _lessonRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IProgresRepository _progressRepository;
    private readonly IUpdateExerciseRepository _updateExerciseRepository;
    private readonly UserManager<PuroUser> _userManager;
    private readonly IAssessExercise _assessor;

    [BindProperty]
    public Lesson Lesson { get; set; }

    [BindProperty]
    public Exercise ExerciseObject { get; set; }

    [BindProperty]
    public string Modele { get; set; }

    [BindProperty]
    public string Solution { get; set; }

    public EditExercise(ILessonRepository lessonRepository, IExerciseRepository exerciseRepository, IProgresRepository progressRepository,  IUpdateExerciseRepository updateExerciseRepository, UserManager<PuroUser> userManager,
        IAssessExercise assessor)
    {
        _lessonRepository = lessonRepository;
        _exerciseRepository = exerciseRepository;
        _progressRepository = progressRepository;
        _updateExerciseRepository = updateExerciseRepository;
        _userManager = userManager;
        _assessor = assessor;
    }

    public async Task OnGetAsync(int lessonId, int exerciseId)
    {
        Lesson = await _lessonRepository.GetLessonAsync(lessonId);
        ExerciseObject = await _exerciseRepository.GetExerciseAsync(exerciseId);
        Modele = ExerciseObject.Modele;
        Solution = ExerciseObject.Solution;
    }

    public async Task<IActionResult> OnPostTestCode([FromBody] EditExerciceTestCodeInputModel inputModel)
    {
        var modele = inputModel.Modele;
        var solution = inputModel.Solution;
        try
        {
            var exerciseResult = await _assessor.AssessForTest(modele, solution);
            var testResults = exerciseResult.TestResults;
            var status = string.Empty;
            if (exerciseResult.Status == ExerciseStatus.Passed)
            {
                status = "Passed";
            }
            else
            {
                status = "Failed";
            }

            var results = testResults.Select(tr => new { Label = tr.Label, Status = tr.Status.ToString(), Error = tr.ErrorMessage }).ToList();

            return new JsonResult(new { Status = status, Results = results });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { Status = "Error", Message = ex.Message });
        }
    }

    public async Task<IActionResult> OnPostSaveChanges([FromBody] EditExerciceSaveChangesInputModel inputModel)
    {
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            await _progressRepository.ResetProgressForAllUsersAsync(inputModel.ExerciseId);
            await _updateExerciseRepository.UpdateExerciseDetailsAsync(inputModel.ExerciseId, inputModel.Titre, inputModel.Enonce, inputModel.IDDifficulte, inputModel.Modele, inputModel.Solution);

            Lesson = await _lessonRepository.GetLessonAsync(inputModel.LessonId);
            ExerciseObject = await _exerciseRepository.GetExerciseAsync(inputModel.ExerciseId);

            return RedirectToPage("/Teachers/EditExercise", new { lessonId = Lesson.IDLecon, exerciseId = ExerciseObject.IDExercice });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Une erreur s'est produite lors de la sauvegarde des modifications.");
        }
    }
}
