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

        public IActionResult OnGet(int lessonId, string sortOrder)
        {
            Lesson = _lessonRepository.GetLesson(lessonId);
            Exercises = _lessonRepository.GetExercisesForLesson(lessonId).ToList();
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
                    Exercises = Exercises.OrderBy(e => e.Position).ToList(); // Default sort by ID
                    break;
            }

            if (Lesson == null)
            {
                return RedirectToPage("/Index");
            }

            ViewData["Count"] = Exercises.Count;
            return Page();
        }

        public IActionResult OnPostUpdateTitle([FromBody] UpdateTitleInputModel inputModel)
        {
            var lesson = _lessonRepository.GetLesson(inputModel.LessonId);
            if (lesson == null)
            {
                return new JsonResult(new { success = false, message = "Leçon non trouvée" });
            }

            if (inputModel.Title.Trim().Length == 0)
            {
                return new JsonResult(new { success = false, message = "Le titre ne peut pas être vide" });
            }

            lesson.Intitule = inputModel.Title;
            _updateLessonRepository.UpdateLesson(lesson);

            return new JsonResult(new { success = true });
        }

        public IActionResult OnPostUpdateDescription([FromBody] UpdateDescriptionInputModel inputModel)
        {
            var lesson = _lessonRepository.GetLesson(inputModel.LessonId);
            if (lesson == null)
            {
                return new JsonResult(new { success = false, message = "Leçon non trouvée" });
            }

            lesson.Description = inputModel.NewDescription;
            _updateLessonRepository.UpdateLesson(lesson);

            return new JsonResult(new { success = true });
        }

        public IActionResult OnPostMoveExerciseDown(int exerciseId)
        {
            _updateExerciseRepository.MoveExerciseDown(exerciseId);
            return RedirectToPage();
        }

        public IActionResult OnPostMoveExerciseUp(int exerciseId)
        {
            _updateExerciseRepository.MoveExerciseUp(exerciseId);
            return RedirectToPage();
        }

        public IActionResult OnPostHideExercise(int exerciseId)
        {
            _updateExerciseRepository.HideExercise(exerciseId);
            return RedirectToPage();
        }

        public IActionResult OnPostUnHideExercise(int exerciseId)
        {
            _updateExerciseRepository.UnHideExercise(exerciseId);
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteExercise(int exerciseId)
        {
            _updateExerciseRepository.DeleteExercise(exerciseId);
            return RedirectToPage();
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostCreateExercise(int lessonId)
        {
            Console.WriteLine(CreateExerciseInputModel.NewExerciseName);
            var exercises = _lessonRepository.GetExercisesForLesson(lessonId).ToList();

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

            var newExerciseId = _updateExerciseRepository.CreateExercise(CreateExerciseInputModel.NewExerciseName, lessonId);
            return RedirectToPage("EditExercise", new { lessonId = lessonId, exerciseId = newExerciseId });
        }
    }
}
