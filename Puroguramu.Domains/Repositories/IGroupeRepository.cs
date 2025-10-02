using Puroguramu.Domains.Models;

namespace Puroguramu.Domains.Repositories;

public interface IGroupeRepository
{
    Task<List<Group>> GetAllGroupesAsync();

    Task<Group> GetGroupeNameAsync(int idGroupe);
}
