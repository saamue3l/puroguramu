using Puroguramu.Domains;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.Infrastructures.Repositories;

public class ExerciseRepository : IExerciseRepository
{

    private readonly PuroguramuDbContext _context;

    public ExerciseRepository(PuroguramuDbContext context)
    {
        _context = context;
    }

    public Exercise GetExercise(int exerciseId)
    {
        return _context.Exercises
            .FirstOrDefault(e => e.IDExercice == exerciseId);
    }
}
