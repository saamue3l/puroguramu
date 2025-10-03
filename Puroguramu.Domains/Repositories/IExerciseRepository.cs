using Puroguramu.Domains.Models;

namespace Puroguramu.Domains.Repositories;

public interface IExerciseRepository
{
    Task<Exercise> GetExerciseAsync(int exerciseId);
}
