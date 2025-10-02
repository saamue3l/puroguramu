using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Puroguramu.Infrastructures.DbContexts;

public class ProdPuroguramuDbContext : PuroguramuDbContext
{

    public ProdPuroguramuDbContext(IConfiguration configuration) : base(configuration)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("PuroguramuDbContext"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}
