using EventRep.Domain.Contracts;
using EventRep.Domain.Entity;
using EventRep.Infrastructure.Persistence.Mappers;
using EventRep.Infrastructure.Persistence.Models;

namespace EventRep.Infrastructure.Persistence.Repositories;

internal sealed class PortfolioRepository(EventRepDbContext dbContext)
    : Repository<PortfolioEntity, PortfolioOrm>(dbContext), IPortfolioRepository
{
    protected override PortfolioEntity ToDomain(PortfolioOrm model) => model.ToDomain();

    protected override PortfolioOrm ToOrm(PortfolioEntity entity) => entity.ToOrm();

    public Task<PortfolioEntity?> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default) =>
        ToDomainSingleOrDefaultAsync(
            Set.Where(portfolio => portfolio.UserId == userId),
            cancellationToken);
}
