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

        public IActionResult OnGet(string sortOrder)
        {
            var lessons = _repository.GetAllLessons();
            var studentsCompletedLesson = new Dictionary<int, int>();
            var totalStudents = _repository.GetTotalStudents();
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
                    lessons = lessons.OrderBy(l => l.Position).ToList(); // Default sort by ID
                    break;
            }

            foreach (var lesson in lessons)
            {
                studentsCompletedLesson[lesson.IDLecon] = _repository.GetNbStudentHasCompletedLesson(lesson.IDLecon);
            }

            LessonCardViewModels = lessons.Select(lesson => new LessonCardViewModel
            {
                Lesson = lesson, StudentsCompletedLesson = studentsCompletedLesson, TotalStudents = totalStudents, Count = lessons.Count
            }).ToList();

            return Page();
        }

        public IActionResult OnPostMoveLessonUp(int lessonId)
        {
            _updateLessonRepository.MoveLessonUp(lessonId);
            return RedirectToPage();
        }

        public IActionResult OnPostMoveLessonDown(int lessonId)
        {
            _updateLessonRepository.MoveLessonDown(lessonId);
            return RedirectToPage();
        }

        public IActionResult OnPostHideLesson(int lessonId)
        {
            _updateLessonRepository.HideLesson(lessonId);
            return RedirectToPage();
        }

        public IActionResult OnPostUnhideLesson(int lessonId)
        {
            _updateLessonRepository.UnHideLesson(lessonId);
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteLesson(int lessonId)
        {
            _updateLessonRepository.DeleteLesson(lessonId);
            return RedirectToPage();
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostCreateLesson(int lessonId)
        {
            if (!ModelState.IsValid)//if the model (CreateLessonInputModel) is not valid, return to the page
            {
                TempData["ErrorMessage"] = "Veuillez fournir un nom de leçon valide.";
                return RedirectToPage();
            }

            var lessons = _repository.GetAllLessons().ToList();

            if (lessons.Any(l => l.Intitule.ToLower() == CreateLessonInputModel.NewLessonName.ToLower()))
            {
                TempData["ErrorMessage"] = "Une leçon avec ce nom existe déjà.";
                return RedirectToPage();
            }

            var newLessonId = _updateLessonRepository.CreateLesson(CreateLessonInputModel.NewLessonName);
            return RedirectToPage("EditLesson", new { lessonId = newLessonId });
        }


        public int GetStudentHasCompletedLesson(int lessonId)
        {
            return _repository.GetNbStudentHasCompletedLesson(lessonId);
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
