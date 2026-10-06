using EventRep.Domain.Entity;

namespace EventRep.Domain.Contracts;

public interface IEventFeedbackRepository : IRepository<EventFeedbackEntity>
{
    Task<List<EventFeedbackEntity>> GetByEventIdAsync(
        Guid eventId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid eventId,
        Guid executorId,
        CancellationToken cancellationToken = default);
}
