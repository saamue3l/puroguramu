using System.Globalization;
using Puroguramu.App.Constants;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;

namespace Puroguramu.App.Services;

public interface IProgressService
{
    Task<Progress> GetOrCreateProgressAsync(int exerciseId, string userId, string exerciseStub);
    Task UpdateProgressAttemptAsync(int exerciseId, string userId, string code);
    Task UpdateProgressStatusAsync(int exerciseId, string userId, ProgressStatusEnum status);
    Task<bool> CanUpdateProgressAsync(int exerciseId, string userId);
}

public class ProgressService : IProgressService
{
    private readonly IProgresRepository _progresRepository;

    public ProgressService(IProgresRepository progresRepository)
    {
        _progresRepository = progresRepository;
    }

    public async Task<Progress> GetOrCreateProgressAsync(int exerciseId, string userId, string exerciseStub)
    {
        var progress = await _progresRepository.GetProgresAsync(exerciseId, userId);

        if (progress == null)
        {
            progress = new Progress
            {
                IDExercice = exerciseId,
                IDUtilisateur = userId,
                IDStatut = (int)ProgressStatusEnum.NotStarted,
                CodeDerniereTentative = exerciseStub,
                DateDerniereTentative = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            };
            await _progresRepository.CreateProgresAsync(progress);
        }

        return progress;
    }

    public async Task UpdateProgressAttemptAsync(int exerciseId, string userId, string code)
    {
        await _progresRepository.UpdateProgresAttemptAsync(exerciseId, userId, code);
    }

    public async Task UpdateProgressStatusAsync(int exerciseId, string userId, ProgressStatusEnum status)
    {
        await _progresRepository.UpdateProgresStatusAsync(exerciseId, userId, (int)status);
    }

    public async Task<bool> CanUpdateProgressAsync(int exerciseId, string userId)
    {
        var progress = await _progresRepository.GetProgresAsync(exerciseId, userId);

        if (progress == null)
            return false;

        var status = (ProgressStatusEnum)progress.IDStatut;
        return status == ProgressStatusEnum.NotStarted || status == ProgressStatusEnum.InProgress;
    }
}
