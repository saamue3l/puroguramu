using Microsoft.EntityFrameworkCore;
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

    public async Task<Exercise> GetExerciseAsync(int exerciseId)
    {
        return await _context.Exercises
            .FirstOrDefaultAsync(e => e.IDExercice == exerciseId);
    }
}
