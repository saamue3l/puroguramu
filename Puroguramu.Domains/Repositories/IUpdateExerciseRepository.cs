using Puroguramu.Domains.Models;

namespace Puroguramu.Domains.Repositories;

public interface IUpdateExerciseRepository
{
        int CreateExercise(string Title, int IdLecon);

        void MoveExerciseUp(int exerciseId);

        void MoveExerciseDown(int exerciseId);

        void UpdateExerciseDetails(int exerciseId, string titre, string enonce, int iDDifficulte, string modele, string solution);

        void HideExercise(int exerciseId);

        void UnHideExercise(int exerciseId);

        void DeleteExercise(int exerciseId);
}
