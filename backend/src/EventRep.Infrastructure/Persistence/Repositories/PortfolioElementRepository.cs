using EventRep.Domain.Contracts;
using EventRep.Domain.Entity;
using EventRep.Infrastructure.Persistence.Mappers;
using EventRep.Infrastructure.Persistence.Models;

namespace EventRep.Infrastructure.Persistence.Repositories;

internal sealed class PortfolioElementRepository(EventRepDbContext dbContext)
    : Repository<PortfolioElementEntity, PortfolioElementOrm>(dbContext),
        IPortfolioElementRepository
{
    protected override PortfolioElementEntity ToDomain(PortfolioElementOrm model) =>
        model.ToDomain();

    protected override PortfolioElementOrm ToOrm(PortfolioElementEntity entity) =>
        entity.ToOrm();

    public Task<List<PortfolioElementEntity>> GetByPortfolioIdAsync(
        Guid portfolioId,
        CancellationToken cancellationToken = default) =>
        ToDomainListAsync(
            Set.Where(element => element.PortfolioId == portfolioId)
                .OrderByDescending(element => element.CreatedAt),
            cancellationToken);
}
