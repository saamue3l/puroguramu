using Puroguramu.Domains.Models;

namespace Puroguramu.Domains.Repositories;

public interface IProgresRepository
{
    public Task<Progress> GetProgresAsync(int exerciceId, string userId);
    Task<Progress> CreateProgresAsync(Progress newProgress);
    Task<Progress> UpdateProgresStatusAsync(int exerciceId, string userId, int newStatus);
    Task<Progress> UpdateProgresAttemptAsync(int exerciceId, string userId, string attempt);
    Task ResetProgressForAllUsersAsync(int exerciceId);

}
