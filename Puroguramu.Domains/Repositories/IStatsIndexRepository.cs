namespace Puroguramu.Domains.Repositories;

public interface IStatsIndexRepository
{
    public int LessonCount { get; }

    public int ExerciseCount { get; }
}
