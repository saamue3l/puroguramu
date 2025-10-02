using Puroguramu.Domains.Models;

namespace Puroguramu.Domains.Repositories;

public interface IExerciseRepository
{
    Exercise GetExercise(int exerciseId);
}
