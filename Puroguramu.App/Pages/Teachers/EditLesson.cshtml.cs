using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Puroguramu.App.Pages.Teachers.EntryModels;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;

namespace Puroguramu.App.Pages.Teachers
{
    public class EditLesson : PageModel
    {
        private readonly ILessonRepository _lessonRepository;

        private readonly IUpdateLessonRepository _updateLessonRepository;

        private readonly IUpdateExerciseRepository _updateExerciseRepository;

        public Lesson? Lesson { get; set; }

        public List<Exercise>? Exercises { get; set; }

        [BindProperty]
        public  CreateExerciseInputModel CreateExerciseInputModel { get; set; }

        [BindProperty]
        UpdateDescriptionInputModel UpdateDescriptionInputModel { get; set; }

        public EditLesson(ILessonRepository repository, IUpdateLessonRepository updateLessonRepository, IUpdateExerciseRepository updateExerciseRepository)
        {
            _lessonRepository = repository;
            _updateLessonRepository = updateLessonRepository;
            _updateExerciseRepository = updateExerciseRepository;
        }

        public async Task<IActionResult> OnGetAsync(int lessonId, string sortOrder)
        {
            Lesson = await _lessonRepository.GetLessonAsync(lessonId);
            Exercises = (await _lessonRepository.GetExercisesForLessonAsync(lessonId)).ToList();
            switch (sortOrder)
            {
                case "title":
                    Exercises = Exercises.OrderBy(e => e.Titre).ToList();
                    break;
                case "status":
                    Exercises = Exercises.OrderByDescending(e => e.IDStatut).ToList();
                    break;
                case "position":
                    Exercises = Exercises.OrderBy(e => e.Position).ToList();
                    break;
                default:
                    Exercises = Exercises.OrderBy(e => e.Position).ToList();
                    break;
            }

            if (Lesson == null)
            {
                return RedirectToPage("/Index");
            }

            ViewData["Count"] = Exercises.Count;
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateTitleAsync([FromBody] UpdateTitleInputModel inputModel)
        {
            var lesson = await _lessonRepository.GetLessonAsync(inputModel.LessonId);
            if (lesson == null)
            {
                return new JsonResult(new { success = false, message = "Leçon non trouvée" });
            }

            if (inputModel.Title.Trim().Length == 0)
            {
                return new JsonResult(new { success = false, message = "Le titre ne peut pas être vide" });
            }

            lesson.Intitule = inputModel.Title;
            await _updateLessonRepository.UpdateLessonAsync(lesson);

            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostUpdateDescriptionAsync([FromBody] UpdateDescriptionInputModel inputModel)
        {
            var lesson = await _lessonRepository.GetLessonAsync(inputModel.LessonId);
            if (lesson == null)
            {
                return new JsonResult(new { success = false, message = "Leçon non trouvée" });
            }

            lesson.Description = inputModel.NewDescription;
            await _updateLessonRepository.UpdateLessonAsync(lesson);

            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostMoveExerciseDownAsync(int exerciseId)
        {
            await _updateExerciseRepository.MoveExerciseDownAsync(exerciseId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostMoveExerciseUpAsync(int exerciseId)
        {
            await _updateExerciseRepository.MoveExerciseUpAsync(exerciseId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostHideExerciseAsync(int exerciseId)
        {
            await _updateExerciseRepository.HideExerciseAsync(exerciseId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUnHideExerciseAsync(int exerciseId)
        {
            await _updateExerciseRepository.UnHideExerciseAsync(exerciseId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteExerciseAsync(int exerciseId)
        {
            await _updateExerciseRepository.DeleteExerciseAsync(exerciseId);
            return RedirectToPage();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostCreateExerciseAsync(int lessonId)
        {
            Console.WriteLine(CreateExerciseInputModel.NewExerciseName);
            var exercises = (await _lessonRepository.GetExercisesForLessonAsync(lessonId)).ToList();

            if (string.IsNullOrWhiteSpace(CreateExerciseInputModel.NewExerciseName) || !ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Un exercice ne peut pas avoir un nom vide.";
                return RedirectToPage();
            }

            if (exercises.Any(e => e.Titre.ToLower() == CreateExerciseInputModel.NewExerciseName.ToLower()))
            {
                TempData["ErrorMessage"] = "Un exercice avec ce nom existe déjà dans la leçon.";
                return RedirectToPage();
            }

            var newExerciseId = await _updateExerciseRepository.CreateExerciseAsync(CreateExerciseInputModel.NewExerciseName, lessonId);
            return RedirectToPage("EditExercise", new { lessonId = lessonId, exerciseId = newExerciseId });
        }
    }
}
