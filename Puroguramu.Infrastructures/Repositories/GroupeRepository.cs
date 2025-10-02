using Microsoft.EntityFrameworkCore;
using Puroguramu.Domains;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.Infrastructures.Repositories;

public class GroupeRepository : IGroupeRepository
{
    private readonly PuroguramuDbContext _context;

    public GroupeRepository(PuroguramuDbContext context)
    {
        _context = context;
    }

    public async Task<List<Group>> GetAllGroupesAsync()
    {
        return await _context.Groups.ToListAsync();
    }

    public async Task<Group> GetGroupeNameAsync(int idGroupe)
    {
        var groupe = await _context.Groups.FirstOrDefaultAsync(g => g.IDGroupe == idGroupe);
        return groupe;
    }
}
