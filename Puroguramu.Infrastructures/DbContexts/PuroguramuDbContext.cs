using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Puroguramu.Domains;
using Puroguramu.Domains.Models;
using Puroguramu.Infrastructures.DbContexts.Configurations;

namespace Puroguramu.Infrastructures.DbContexts;

public class PuroguramuDbContext : IdentityDbContext<PuroUser>
{

    protected readonly IConfiguration Configuration;

    public PuroguramuDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(Configuration.GetConnectionString("PuroguramuDbContext"));
    }

    public DbSet<Difficulty> Difficulties { get; set; }

    public DbSet<Group> Groups { get; set; }

    public DbSet<Statut> Status { get; set; }

    public DbSet<Lesson> Lessons { get; set; }

    public DbSet<Exercise> Exercises { get; set; }

    public DbSet<ProgressStatus> ProgressStatus { get; set; }


    public DbSet<Progress> Progress { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StatutProgresConfiguration());
        modelBuilder.ApplyConfiguration(new DifficulteConfiguration());
        modelBuilder.ApplyConfiguration(new GroupeConfiguration());
        modelBuilder.ApplyConfiguration(new StatutConfiguration());
        modelBuilder.ApplyConfiguration(new LeconConfiguration());
        modelBuilder.ApplyConfiguration(new ExerciseConfiguration());
        modelBuilder.ApplyConfiguration(new ProgresConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
