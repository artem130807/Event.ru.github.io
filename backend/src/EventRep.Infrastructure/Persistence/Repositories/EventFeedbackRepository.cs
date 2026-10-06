using EventRep.Domain.Contracts;
using EventRep.Domain.Entity;
using EventRep.Infrastructure.Persistence.Mappers;
using EventRep.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace EventRep.Infrastructure.Persistence.Repositories;

internal sealed class EventFeedbackRepository(EventRepDbContext dbContext)
    : Repository<EventFeedbackEntity, EventFeedbackOrm>(dbContext),
        IEventFeedbackRepository
{
    protected override EventFeedbackEntity ToDomain(EventFeedbackOrm model) =>
        model.ToDomain();

    protected override EventFeedbackOrm ToOrm(EventFeedbackEntity entity) =>
        entity.ToOrm();

    public Task<List<EventFeedbackEntity>> GetByEventIdAsync(
        Guid eventId,
        CancellationToken cancellationToken = default) =>
        ToDomainListAsync(
            Set.Where(feedback => feedback.EventId == eventId)
                .OrderBy(feedback => feedback.CreatedAt),
            cancellationToken);

    public Task<bool> ExistsAsync(
        Guid eventId,
        Guid executorId,
        CancellationToken cancellationToken = default) =>
        Set.AnyAsync(
            feedback => feedback.EventId == eventId
                && feedback.ExecutorId == executorId,
            cancellationToken);
}
