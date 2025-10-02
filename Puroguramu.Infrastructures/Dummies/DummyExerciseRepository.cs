using Puroguramu.Domains;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;

namespace Puroguramu.Infrastructures.Dummies;

public class DummyExerciseRepository : IExerciseRepository
{
    public Exercise GetExercise(int exerciseId)
        => new Exercise();
}
