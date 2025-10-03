using Puroguramu.Domains.Models;

namespace Puroguramu.Domains.Repositories;

public interface IUpdateExerciseRepository
{
        Task<int> CreateExerciseAsync(string Title, int IdLecon);

        Task MoveExerciseUpAsync(int exerciseId);

        Task MoveExerciseDownAsync(int exerciseId);

        Task UpdateExerciseDetailsAsync(int exerciseId, string titre, string enonce, int iDDifficulte, string modele, string solution);

        Task HideExerciseAsync(int exerciseId);

        Task UnHideExerciseAsync(int exerciseId);

        Task DeleteExerciseAsync(int exerciseId);
}
