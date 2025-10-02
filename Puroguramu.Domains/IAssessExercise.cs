using Puroguramu.Domains.Models;

namespace Puroguramu.Domains;

public interface IAssessExercise
{
    Task<ExerciseResult> Assess(int exerciseId, string proposal);
    Task<ExerciseResult> AssessForTest(string modele, string solution);

    Task<ExerciseResult> StubForExercise(int exerciseId);
}
