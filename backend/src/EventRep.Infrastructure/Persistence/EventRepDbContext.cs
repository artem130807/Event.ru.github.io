using EventRep.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace EventRep.Infrastructure.Persistence;

public sealed class EventRepDbContext(DbContextOptions<EventRepDbContext> options)
    : DbContext(options)
{
    public DbSet<UserOrm> Users => Set<UserOrm>();
    public DbSet<EventOrm> Events => Set<EventOrm>();
    public DbSet<PortfolioElementOrm> PortfolioElements => Set<PortfolioElementOrm>();
    public DbSet<ResumeOrm> Resumes => Set<ResumeOrm>();
    public DbSet<PortfolioOrm> Portfolios => Set<PortfolioOrm>();
    public DbSet<EventFeedbackOrm> EventFeedbacks => Set<EventFeedbackOrm>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventRepDbContext).Assembly);
    }
}
