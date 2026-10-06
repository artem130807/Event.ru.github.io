using EventRep.Domain.Enums;

namespace EventRep.Domain.Contracts;

public interface IEventRepository : IRepository<EventEntity>
{
    Task<List<EventEntity>> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default);

    Task<List<EventEntity>> GetByExecutorIdAsync(
        Guid executorId,
        CancellationToken cancellationToken = default);

    Task<List<EventEntity>> GetByStatusAsync(
        EventStatus status,
        CancellationToken cancellationToken = default);
}
