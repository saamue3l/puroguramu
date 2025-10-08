using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Puroguramu.App.Pages.Teachers.EntryModels;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;

namespace Puroguramu.App.Pages.Teachers
{

    [Authorize(Roles = "Enseignant")]
    public class Index : PageModel
    {
        private readonly ILessonRepository _repository;

        private readonly IUpdateLessonRepository _updateLessonRepository;

        private readonly List<Exercise> Exercises;

        public List<LessonCardViewModel> LessonCardViewModels { get; set; }

        [BindProperty] public CreateLessonInputModel CreateLessonInputModel { get; set; }

        public Index(ILessonRepository repository, IUpdateLessonRepository updateLessonRepository)
        {
            _repository = repository;
            _updateLessonRepository = updateLessonRepository;
        }

        public async Task<IActionResult> OnGetAsync(string sortOrder)
        {
            var lessons = await _repository.GetAllLessonsAsync();
            var studentsCompletedLesson = new Dictionary<int, int>();
            var totalStudents = await _repository.GetTotalStudentsAsync();
            switch (sortOrder)
            {
                case "title":
                    lessons = lessons.OrderBy(l => l.Intitule).ToList();
                    break;
                case "status":
                    lessons = lessons.OrderByDescending(l => l.IDStatut).ToList();
                    break;
                case "position":
                    lessons = lessons.OrderBy(l => l.Position).ToList();
                    break;
                default:
                    lessons = lessons.OrderBy(l => l.Position).ToList();
                    break;
            }

            foreach (var lesson in lessons)
            {
                studentsCompletedLesson[lesson.IDLecon] = await _repository.GetNbStudentHasCompletedLessonAsync(lesson.IDLecon);
            }

            LessonCardViewModels = lessons.Select(lesson => new LessonCardViewModel
            {
                Lesson = lesson, StudentsCompletedLesson = studentsCompletedLesson, TotalStudents = totalStudents, Count = lessons.Count
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostMoveLessonUpAsync(int lessonId)
        {
            await _updateLessonRepository.MoveLessonUpAsync(lessonId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostMoveLessonDownAsync(int lessonId)
        {
            await _updateLessonRepository.MoveLessonDownAsync(lessonId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostHideLessonAsync(int lessonId)
        {
            await _updateLessonRepository.HideLessonAsync(lessonId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUnhideLessonAsync(int lessonId)
        {
            await _updateLessonRepository.UnHideLessonAsync(lessonId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteLessonAsync(int lessonId)
        {
            await _updateLessonRepository.DeleteLessonAsync(lessonId);
            return RedirectToPage();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostCreateLessonAsync(int lessonId)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Veuillez fournir un nom de leçon valide.";
                return RedirectToPage();
            }

            var lessons = (await _repository.GetAllLessonsAsync()).ToList();

            if (lessons.Any(l => l.Intitule.ToLower() == CreateLessonInputModel.NewLessonName.ToLower()))
            {
                TempData["ErrorMessage"] = "Une leçon avec ce nom existe déjà.";
                return RedirectToPage();
            }

            var newLessonId = await _updateLessonRepository.CreateLessonAsync(CreateLessonInputModel.NewLessonName);
            return RedirectToPage("EditLesson", new { lessonId = newLessonId });
        }


        public async Task<int> GetStudentHasCompletedLessonAsync(int lessonId)
        {
            return await _repository.GetNbStudentHasCompletedLessonAsync(lessonId);
        }
    }

    public class LessonCardViewModel
    {
        public Lesson Lesson { get; set; }
        public Dictionary<int, int> StudentsCompletedLesson { get; set; }
        public int TotalStudents { get; set; }

        public int Count { get; set; }
    }
}
