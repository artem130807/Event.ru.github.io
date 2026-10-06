using EventRep.Domain.Entity;

namespace EventRep.Domain.Contracts;

public interface IPortfolioElementRepository : IRepository<PortfolioElementEntity>
{
    Task<List<PortfolioElementEntity>> GetByPortfolioIdAsync(
        Guid portfolioId,
        CancellationToken cancellationToken = default);
}
