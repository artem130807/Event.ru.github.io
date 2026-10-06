using EventRep.Domain.Contracts;
using EventRep.Domain.Entity;
using EventRep.Infrastructure.Persistence.Mappers;
using EventRep.Infrastructure.Persistence.Models;

namespace EventRep.Infrastructure.Persistence.Repositories;

internal sealed class ResumeRepository(EventRepDbContext dbContext)
    : Repository<ResumeEntity, ResumeOrm>(dbContext), IResumeRepository
{
    protected override ResumeEntity ToDomain(ResumeOrm model) => model.ToDomain();

    protected override ResumeOrm ToOrm(ResumeEntity entity) => entity.ToOrm();

    public Task<ResumeEntity?> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default) =>
        ToDomainSingleOrDefaultAsync(
            Set.Where(resume => resume.UserId == userId),
            cancellationToken);
}
