using EventRep.Domain.Contracts;
using EventRep.Domain.Enums;
using EventRep.Infrastructure.Persistence.Mappers;
using EventRep.Infrastructure.Persistence.Models;

namespace EventRep.Infrastructure.Persistence.Repositories;

internal sealed class EventRepository(EventRepDbContext dbContext)
    : Repository<EventEntity, EventOrm>(dbContext), IEventRepository
{
    protected override EventEntity ToDomain(EventOrm model) => model.ToDomain();

    protected override EventOrm ToOrm(EventEntity entity) => entity.ToOrm();

    public Task<List<EventEntity>> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default) =>
        ToDomainListAsync(
            Set.Where(eventOrm => eventOrm.CustomerId == customerId)
                .OrderByDescending(eventOrm => eventOrm.FullDate),
            cancellationToken);

    public Task<List<EventEntity>> GetByExecutorIdAsync(
        Guid executorId,
        CancellationToken cancellationToken = default) =>
        ToDomainListAsync(
            Set.Where(eventOrm => eventOrm.ExecutorId == executorId)
                .OrderByDescending(eventOrm => eventOrm.FullDate),
            cancellationToken);

    public Task<List<EventEntity>> GetByStatusAsync(
        EventStatus status,
        CancellationToken cancellationToken = default) =>
        ToDomainListAsync(
            Set.Where(eventOrm => eventOrm.Status == status)
                .OrderBy(eventOrm => eventOrm.FullDate),
            cancellationToken);
}
