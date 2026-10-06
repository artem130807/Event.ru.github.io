using EventRep.Domain.Entity;

namespace EventRep.Domain.Contracts;

public interface IResumeRepository : IRepository<ResumeEntity>
{
    Task<ResumeEntity?> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
