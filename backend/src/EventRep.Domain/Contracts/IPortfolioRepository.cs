using EventRep.Domain.Entity;

namespace EventRep.Domain.Contracts;

public interface IPortfolioRepository : IRepository<PortfolioEntity>
{
    Task<PortfolioEntity?> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
