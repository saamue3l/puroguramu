using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Lesson> GetLessonAsync(int lessonId)
        {
            return await _database.Lessons.FindAsync(lessonId);
        }

        public async Task<IEnumerable<Exercise>> GetExercisesForLessonAsync(int lessonId)
        {
            return await _database.Exercises.Where(e => e.IDLecon == lessonId).OrderBy(e => e.Position).ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetExercisesForLessonStudentAsync(int lessonId)
        {
            return await _database.Exercises.Where(e => e.IDLecon == lessonId && e.IDStatut == 2).OrderBy(e => e.Position).ToListAsync();
        }

        public async Task<List<Lesson>> GetAllLessonsAsync()
        {
            return await _database.Lessons.OrderBy(l => l.Position).ToListAsync();
        }

        public async Task<int> GetNbStudentHasCompletedLessonAsync(int lessonId)
        {
            var exercises = await _database.Exercises.Where(e => e.IDLecon == lessonId).ToListAsync();

            var students = await _userManager.GetUsersInRoleAsync("Etudiant");

            int count = 0;

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

        public async Task<int> GetTotalStudentsAsync()
        {
            var students = await _userManager.GetUsersInRoleAsync("Etudiant");
            return students.Count;
        }

        public async Task<(int TotalExercises, int CompletedExercises)> GetLessonProgressAsync(int lessonId, string userId)
        {
            var exercisesForLesson = await _database.Exercises.Where(e => e.IDLecon == lessonId && e.IDStatut == 2).ToListAsync();

            var totalExercises = exercisesForLesson.Count;

            var completedExercises = exercisesForLesson.Count(e => _database.Progress.Any(p => p.IDExercice == e.IDExercice && p.IDUtilisateur == userId && (p.IDStatut == 3 || p.IDStatut == 4)));

            return (totalExercises, completedExercises);
        }

        public async Task<Exercise> GetNextExerciseAsync(int currentLessonId, int currentExerciseId)
        {
            var currentExercise = await _database.Exercises.FindAsync(currentExerciseId);

            var exercisesForCurrentLesson = await _database.Exercises.Where(e => e.IDLecon == currentLessonId).OrderBy(e => e.Position).ToListAsync();

            var currentIndex = exercisesForCurrentLesson.FindIndex(e => e.IDExercice == currentExerciseId);

            if (currentIndex < exercisesForCurrentLesson.Count - 1)
            {
                return exercisesForCurrentLesson[currentIndex + 1];
            }

            var nextLesson = await _database.Lessons.Where(l => l.IDLecon > currentLessonId).OrderBy(l => l.IDLecon).FirstOrDefaultAsync();

            if (nextLesson != null)
            {
                return await _database.Exercises.Where(e => e.IDLecon == nextLesson.IDLecon).OrderBy(e => e.Position).FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<Exercise> GetNextUncompletedExerciseAsync(string userId)
        {
            var allExercises = await _database.Exercises.Where(e => e.IDStatut == 2).OrderBy(e => e.IDLecon).ThenBy(e => e.Position).ToListAsync();

            foreach (var exercise in allExercises)
            {

                var progress = await _database.Progress.FirstOrDefaultAsync(p => p.IDExercice == exercise.IDExercice && p.IDUtilisateur == userId);

                if (progress == null || (progress.IDStatut != 3 && progress.IDStatut != 4))
                {
                    return exercise;
                }
            }

            return null;
        }

        public async Task<Exercise> GetLastAttemptedExerciseAsync(string userId)
        {
            var lastProgress = await _database.Progress
                .Where(p => p.IDUtilisateur == userId &&  p.IDStatut == 2)
                .OrderByDescending(p => p.DateDerniereTentative)
                .FirstOrDefaultAsync();

            if (lastProgress != null)
            {
                return await _database.Exercises.FindAsync(lastProgress.IDExercice);
            }

            return null;
        }
    }
}
