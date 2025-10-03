using Microsoft.EntityFrameworkCore;
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

    public async Task MoveExerciseUpAsync(int exerciseId)
    {
        var exercise = await _context.Exercises.FindAsync(exerciseId);
        var previousExercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Position == exercise.Position - 1 && e.IDLecon == exercise.IDLecon);
        if (previousExercise != null)
        {
            previousExercise.Position += 1;
            exercise.Position -= 1;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MoveExerciseDownAsync(int exerciseId)
    {
        var exercise = await _context.Exercises.FindAsync(exerciseId);
        var nextExercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Position == exercise.Position + 1 && e.IDLecon == exercise.IDLecon);
        if (nextExercise != null)
        {
            nextExercise.Position -= 1;
            exercise.Position += 1;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> CreateExerciseAsync(string Title, int IdLecon)
    {
        var exercise = new Exercise
        {
            Titre = Title,
            Position = await GetNextPositionAsync(IdLecon),
            IDLecon = IdLecon,
            IDDifficulte = 1,
            IDStatut = 1,
        };
        _context.Exercises.Add(exercise);
        await _context.SaveChangesAsync();

        return exercise.IDExercice;
    }

    private async Task<int> GetNextPositionAsync(int IdLecon)
    {
        var exercises = _context.Exercises.Where(e => e.IDLecon == IdLecon);
        var position = await exercises.AnyAsync() ? await exercises.MaxAsync(e => e.Position) : 0;
        return position + 1;
    }

    public async Task HideExerciseAsync(int exerciseId)
    {
        var exercise = await _context.Exercises.FindAsync(exerciseId);
        exercise.IDStatut = 1;
        await _context.SaveChangesAsync();
    }

    public async Task UnHideExerciseAsync(int exerciseId)
    {
        var exercise = await _context.Exercises.FindAsync(exerciseId);
        exercise.IDStatut = 2;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteExerciseAsync(int exerciseId)
    {
        var exercise = await _context.Exercises.FindAsync(exerciseId);
        var followingExercises = await _context.Exercises.Where(e => e.Position > exercise.Position && e.IDLecon == exercise.IDLecon).ToListAsync();
        foreach (var followingExercise in followingExercises)
        {
            followingExercise.Position -= 1;
        }

        _context.Exercises.Remove(exercise);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateExerciseDetailsAsync(int exerciseId, string titre, string enonce, int iDDifficulte, string modele, string solution)
    {
        var exercise = await _context.Exercises.FindAsync(exerciseId);

        if (exercise != null)
        {
            exercise.Titre = titre;
            exercise.Enonce = enonce;
            exercise.IDDifficulte = iDDifficulte;
            exercise.Modele = modele;
            exercise.Solution = solution;
            await _context.SaveChangesAsync();
        }
    }
}
