using Puroguramu.Domains;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.Infrastructures;

public class StatsIndexRepository : IStatsIndexRepository
{
    private readonly PuroguramuDbContext _database;

    public StatsIndexRepository(PuroguramuDbContext database)
    {
        _database = database;
    }

    public int LessonCount => _database.Lessons.Count();

    public int ExerciseCount => _database.Exercises.Count();
}
