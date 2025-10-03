using Microsoft.EntityFrameworkCore;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;
using Progress = Puroguramu.Domains.Models.Progress;

namespace Puroguramu.Infrastructures.Repositories;

public class ProgresRepository : IProgresRepository
{
    private readonly PuroguramuDbContext _database;

    public ProgresRepository(PuroguramuDbContext database)
    {
        _database = database;
    }

    public async Task<Progress> GetProgresAsync(int exerciceId, string userId)
    {
        var dbProgres = await _database.Progress
            .Include(p => p.ProgressStatus)
            .FirstOrDefaultAsync(p => p.IDExercice == exerciceId && p.IDUtilisateur == userId);
        if (dbProgres == null)
        {
            return null;
        }

        return new Progress
        {
            IDProgres = dbProgres.IDProgres,
            CodeDerniereTentative = dbProgres.CodeDerniereTentative,
            DateDerniereTentative = dbProgres.DateDerniereTentative,
            IDUtilisateur = dbProgres.IDUtilisateur,
            IDStatut = dbProgres.IDStatut,
            IDExercice = dbProgres.IDExercice,
            ProgressStatus = dbProgres.ProgressStatus
        };
    }

    public async Task<Progress> CreateProgresAsync(Progress newProgress)
    {
        var dbProgres = new DbContexts.Progress()
        {
            IDProgres = newProgress.IDProgres,
            CodeDerniereTentative = newProgress.CodeDerniereTentative,
            DateDerniereTentative = newProgress.DateDerniereTentative,
            IDUtilisateur = newProgress.IDUtilisateur,
            IDStatut = newProgress.IDStatut,
            IDExercice = newProgress.IDExercice,
            ProgressStatus = newProgress.ProgressStatus
        };

        _database.Progress.Add(dbProgres);
        await _database.SaveChangesAsync();

        return new Progress
        {
            IDProgres = dbProgres.IDProgres,
            CodeDerniereTentative = dbProgres.CodeDerniereTentative,
            DateDerniereTentative = dbProgres.DateDerniereTentative,
            IDUtilisateur = dbProgres.IDUtilisateur,
            IDStatut = dbProgres.IDStatut,
            IDExercice = dbProgres.IDExercice,
            ProgressStatus = dbProgres.ProgressStatus
        };
    }

    public async Task<Progress> UpdateProgresStatusAsync(int exerciceId, string userId, int newStatus)
    {
        var dbProgres = await _database.Progress
            .FirstOrDefaultAsync(p => p.IDExercice == exerciceId && p.IDUtilisateur == userId);
        if (dbProgres == null)
        {
            return null;
        }

        dbProgres.IDStatut = newStatus;
        dbProgres.DateDerniereTentative = DateTime.Now.ToString();
        await _database.SaveChangesAsync();

        return new Progress
        {
            IDProgres = dbProgres.IDProgres,
            DateDerniereTentative = dbProgres.DateDerniereTentative,
            CodeDerniereTentative = dbProgres.CodeDerniereTentative,
            IDUtilisateur = dbProgres.IDUtilisateur,
            IDStatut = dbProgres.IDStatut,
            IDExercice = dbProgres.IDExercice,
            ProgressStatus = dbProgres.ProgressStatus
        };
    }

    public async Task<Progress> UpdateProgresAttemptAsync(int exerciceId, string userId, string attempt)
    {
        var dbProgres = await _database.Progress
            .FirstOrDefaultAsync(p => p.IDExercice == exerciceId && p.IDUtilisateur == userId);
        if (dbProgres == null)
        {
            return null;
        }

        dbProgres.CodeDerniereTentative = attempt;
        await _database.SaveChangesAsync();

        return new Progress
        {
            IDProgres = dbProgres.IDProgres,
            CodeDerniereTentative = dbProgres.CodeDerniereTentative,
            DateDerniereTentative = dbProgres.DateDerniereTentative,
            IDUtilisateur = dbProgres.IDUtilisateur,
            IDStatut = dbProgres.IDStatut,
            IDExercice = dbProgres.IDExercice,
            ProgressStatus = dbProgres.ProgressStatus
        };
    }

    public async Task ResetProgressForAllUsersAsync(int exerciceId)
    {
        var dbProgres = await _database.Progress
            .Where(p => p.IDExercice == exerciceId && p.IDStatut == 3)
            .ToListAsync();
        foreach (var progres in dbProgres)
        {
            progres.IDStatut = 1;
            progres.CodeDerniereTentative = "";
        }

        await _database.SaveChangesAsync();
    }
}
