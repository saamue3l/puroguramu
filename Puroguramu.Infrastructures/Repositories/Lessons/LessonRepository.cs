using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Puroguramu.Domains.Models;
using Puroguramu.Domains;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.Infrastructures
{
    public class LessonRepository : ILessonRepository
    {
        private readonly PuroguramuDbContext _database;

        private readonly UserManager<PuroUser> _userManager;

        public LessonRepository(PuroguramuDbContext database, UserManager<PuroUser> userManager)
        {
            _database = database;
            _userManager = userManager;
        }

        public Lesson GetLesson(int lessonId)
        {
            return _database.Lessons.Find(lessonId);
        }

        public IEnumerable<Exercise> GetExercisesForLesson(int lessonId)
        {
            return _database.Exercises.Where(e => e.IDLecon == lessonId).OrderBy(e => e.Position).ToList();
        }

        public IEnumerable<Exercise> GetExercisesForLessonStudent(int lessonId)
        {
            return _database.Exercises.Where(e => e.IDLecon == lessonId && e.IDStatut == 2).OrderBy(e => e.Position).ToList();
        }

        public List<Lesson> GetAllLessons()
        {
            return _database.Lessons.OrderBy(l => l.Position).ToList();
        }

        public int GetNbStudentHasCompletedLesson(int lessonId)
        {
            // Récupérer tous les exercices pour la leçon donnée
            var exercises = _database.Exercises.Where(e => e.IDLecon == lessonId).ToList();

            // Récupérer tous les utilisateurs ayant le rôle d'étudiant
            var students = _userManager.GetUsersInRoleAsync("Etudiant").Result;

            int count = 0;

            // Pour chaque étudiant, vérifier si tous les exercices de la leçon ont été terminés
            foreach (var student in students)
            {
                bool allExercisesCompleted = exercises.All(e => _database.Progress.Any(p => p.IDExercice == e.IDExercice && p.IDUtilisateur == student.Id && p.IDStatut == 3 || p.IDStatut == 4));
                if (allExercisesCompleted)
                {
                    count++;
                }
            }

            return count;
        }

        public int GetTotalStudents()
        {
            var students = _userManager.GetUsersInRoleAsync("Etudiant").Result;
            return students.Count;
        }

        public (int TotalExercises, int CompletedExercises) GetLessonProgress(int lessonId, string userId)
        {
            var exercisesForLesson = _database.Exercises.Where(e => e.IDLecon == lessonId && e.IDStatut == 2).ToList();

            var totalExercises = exercisesForLesson.Count;

            var completedExercises = exercisesForLesson.Count(e => _database.Progress.Any(p => p.IDExercice == e.IDExercice && p.IDUtilisateur == userId && (p.IDStatut == 3 || p.IDStatut == 4)));

            return (totalExercises, completedExercises);
        }

        public Exercise GetNextExercise(int currentLessonId, int currentExerciseId)
        {
            var currentExercise = _database.Exercises.Find(currentExerciseId);

            var exercisesForCurrentLesson = _database.Exercises.Where(e => e.IDLecon == currentLessonId).OrderBy(e => e.Position).ToList();

            var currentIndex = exercisesForCurrentLesson.FindIndex(e => e.IDExercice == currentExerciseId);

            if (currentIndex < exercisesForCurrentLesson.Count - 1)
            {
                return exercisesForCurrentLesson[currentIndex + 1];
            }

            var nextLesson = _database.Lessons.Where(l => l.IDLecon > currentLessonId).OrderBy(l => l.IDLecon).FirstOrDefault();

            if (nextLesson != null)
            {
                return _database.Exercises.Where(e => e.IDLecon == nextLesson.IDLecon).OrderBy(e => e.Position).FirstOrDefault();
            }

            return null;
        }

        public Exercise GetNextUncompletedExercise(string userId)
        {
            var allExercises = _database.Exercises.Where(e => e.IDStatut == 2).OrderBy(e => e.IDLecon).ThenBy(e => e.Position).ToList();

            foreach (var exercise in allExercises)
            {

                var progress = _database.Progress.FirstOrDefault(p => p.IDExercice == exercise.IDExercice && p.IDUtilisateur == userId);

                if (progress == null || (progress.IDStatut != 3 && progress.IDStatut != 4))
                {
                    return exercise;
                }
            }

            return null;
        }

        public Exercise GetLastAttemptedExercise(string userId)
        {
            var lastProgress = _database.Progress
                .Where(p => p.IDUtilisateur == userId &&  p.IDStatut == 2)
                .OrderByDescending(p => p.DateDerniereTentative)
                .FirstOrDefault();

            if (lastProgress != null)
            {
                return _database.Exercises.Find(lastProgress.IDExercice);
            }

            return null;
        }
    }
}
