using Puroguramu.Domains.Models;

namespace Puroguramu.Domains.Repositories;

public interface IProgresRepository
{
    public Progress GetProgres(int exerciceId, string userId);
    Progress CreateProgres(Progress newProgress);
    Progress UpdateProgresStatus(int exerciceId, string userId, int newStatus);
    Progress UpdateProgresAttempt(int exerciceId, string userId, string attempt);
    void ResetProgressForAllUsers(int exerciceId);

}
