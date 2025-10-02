using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.Infrastructures.Repositories;

public class UpdateExerciseRepository : IUpdateExerciseRepository
{
    private readonly PuroguramuDbContext _context;

    public UpdateExerciseRepository(PuroguramuDbContext context)
    {
        _context = context;
    }

    public void MoveExerciseUp(int exerciseId)
    {
        var exercise = _context.Exercises.Find(exerciseId);
        var previousExercise = _context.Exercises.FirstOrDefault(e => e.Position == exercise.Position - 1 && e.IDLecon == exercise.IDLecon);
        if (previousExercise != null)
        {
            previousExercise.Position += 1;
            exercise.Position -= 1;
            _context.SaveChanges();
        }
    }

    public void MoveExerciseDown(int exerciseId)
    {
        var exercise = _context.Exercises.Find(exerciseId);
        var nextExercise = _context.Exercises.FirstOrDefault(e => e.Position == exercise.Position + 1 && e.IDLecon == exercise.IDLecon);
        if (nextExercise != null)
        {
            nextExercise.Position -= 1;
            exercise.Position += 1;
            _context.SaveChanges();
        }
    }

    public int CreateExercise(string Title, int IdLecon)
    {
        var exercise = new Exercise
        {
            Titre = Title,
            Position = GetNextPosition(IdLecon),
            IDLecon = IdLecon,
            IDDifficulte = 1,
            IDStatut = 1,
        };
        _context.Exercises.Add(exercise);
        _context.SaveChanges();

        return exercise.IDExercice;
    }

    private int GetNextPosition(int IdLecon)
    {
        var exercises = _context.Exercises.Where(e => e.IDLecon == IdLecon);
        var position = exercises.Any() ? exercises.Max(e => e.Position) : 0;
        return position + 1;
    }

    public void HideExercise(int exerciseId)
    {
        var exercise = _context.Exercises.Find(exerciseId);
        exercise.IDStatut = 1;
        _context.SaveChanges();
    }

    public void UnHideExercise(int exerciseId)
    {
        var exercise = _context.Exercises.Find(exerciseId);
        exercise.IDStatut = 2;
        _context.SaveChanges();
    }

    public void DeleteExercise(int exerciseId)
    {
        var exercise = _context.Exercises.Find(exerciseId);
        var followingExercises = _context.Exercises.Where(e => e.Position > exercise.Position && e.IDLecon == exercise.IDLecon);
        foreach (var followingExercise in followingExercises)
        {
            followingExercise.Position -= 1;
        }

        _context.Exercises.Remove(exercise);
        _context.SaveChanges();
    }

    public void UpdateExerciseDetails(int exerciseId, string titre, string enonce, int iDDifficulte, string modele, string solution)
    {
        var exercise = _context.Exercises.Find(exerciseId);

        if (exercise != null)
        {
            exercise.Titre = titre;
            exercise.Enonce = enonce;
            exercise.IDDifficulte = iDDifficulte;
            exercise.Modele = modele;
            exercise.Solution = solution;
            _context.SaveChanges();
        }
    }
}
