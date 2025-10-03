using Puroguramu.Domains.Models;

namespace Puroguramu.Domains.Repositories;

public interface ILessonRepository
{
    public Task<Lesson> GetLessonAsync(int lessonId);

    public Task<IEnumerable<Exercise>> GetExercisesForLessonAsync(int lessonId);
    public Task<IEnumerable<Exercise>> GetExercisesForLessonStudentAsync(int lessonId);

    public Task<List<Lesson>> GetAllLessonsAsync();

    public Task<(int TotalExercises, int CompletedExercises)> GetLessonProgressAsync(int lessonId, string userId);

    public Task<Exercise> GetNextExerciseAsync(int currentLessonId, int currentExerciseId);

    public Task<Exercise> GetNextUncompletedExerciseAsync(string userId);

    public Task<Exercise> GetLastAttemptedExerciseAsync(string userId);

    public Task<int> GetNbStudentHasCompletedLessonAsync(int lessonId);

    public Task<int> GetTotalStudentsAsync();
}
