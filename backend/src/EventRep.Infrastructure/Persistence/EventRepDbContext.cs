using Microsoft.EntityFrameworkCore;

namespace EventRep.Infrastructure.Persistence;

public sealed class EventRepDbContext(DbContextOptions<EventRepDbContext> options)
    : DbContext(options);
