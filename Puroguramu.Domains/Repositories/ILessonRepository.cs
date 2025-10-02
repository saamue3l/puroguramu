using Puroguramu.Domains.Models;

namespace Puroguramu.Domains.Repositories;

public interface ILessonRepository
{
    public Lesson GetLesson(int lessonId);

    public IEnumerable<Exercise> GetExercisesForLesson(int lessonId);
    public IEnumerable<Exercise> GetExercisesForLessonStudent(int lessonId);

    public List<Lesson> GetAllLessons();

    public (int TotalExercises, int CompletedExercises) GetLessonProgress(int lessonId, string userId);

    public Exercise GetNextExercise(int currentLessonId, int currentExerciseId);

    public Exercise GetNextUncompletedExercise(string userId);

    public Exercise GetLastAttemptedExercise(string userId);

    public int GetNbStudentHasCompletedLesson(int lessonId);

    public int GetTotalStudents();
}
