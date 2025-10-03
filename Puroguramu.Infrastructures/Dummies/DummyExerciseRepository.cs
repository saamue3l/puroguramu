using Puroguramu.Domains;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;

namespace Puroguramu.Infrastructures.Dummies;

public class DummyExerciseRepository : IExerciseRepository
{
    public Task<Exercise> GetExerciseAsync(int exerciseId)
        => Task.FromResult(new Exercise());
}
