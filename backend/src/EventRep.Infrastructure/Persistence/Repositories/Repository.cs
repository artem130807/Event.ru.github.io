using EventRep.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EventRep.Infrastructure.Persistence.Repositories;

internal abstract class Repository<TEntity, TOrm>(EventRepDbContext dbContext)
    : IRepository<TEntity>
    where TEntity : class
    where TOrm : class
{
    protected EventRepDbContext DbContext { get; } = dbContext;

    protected DbSet<TOrm> Set { get; } = dbContext.Set<TOrm>();

    protected abstract TEntity ToDomain(TOrm model);

    protected abstract TOrm ToOrm(TEntity entity);

    protected async Task<List<TEntity>> ToDomainListAsync(
        IQueryable<TOrm> query,
        CancellationToken cancellationToken)
    {
        var models = await query.AsNoTracking().ToListAsync(cancellationToken);
        return models.ConvertAll(model => ToDomain(model));
    }

    protected async Task<TEntity?> ToDomainSingleOrDefaultAsync(
        IQueryable<TOrm> query,
        CancellationToken cancellationToken)
    {
        var model = await query.AsNoTracking().SingleOrDefaultAsync(cancellationToken);
        return model is null ? null : ToDomain(model);
    }

    public virtual async Task<TEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await ToDomainSingleOrDefaultAsync(
            Set.Where(item => EF.Property<Guid>(item, "Id") == id),
            cancellationToken);
    }

    public virtual async Task AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await Set.AddAsync(ToOrm(entity), cancellationToken);
    }

    public virtual void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        Set.Update(ToOrm(entity));
    }

    public virtual void Remove(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        Set.Remove(ToOrm(entity));
    }
}
