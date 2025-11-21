using AvaliadorGuia.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AvaliadorGuia.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Challenge> Challenges => Set<Challenge>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Hint> Hints => Set<Hint>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Candidate>()
            .HasMany(c => c.Sessions)
            .WithOne(s => s.Candidate)
            .HasForeignKey(s => s.CandidateId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Job>()
            .HasMany(j => j.Challenges)
            .WithOne(c => c.Job)
            .HasForeignKey(c => c.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Challenge>()
            .HasMany(c => c.Sessions)
            .WithOne(s => s.Challenge)
            .HasForeignKey(s => s.ChallengeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Session>()
            .HasMany(s => s.Hints)
            .WithOne(h => h.Session)
            .HasForeignKey(h => h.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
