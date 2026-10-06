using EventRep.Application.Contracts.Persistence;
using EventRep.Application.Events.Common;
using EventRep.Domain.Contracts.Pagination;
using EventRep.Domain.Enums;
using EventRep.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventRep.Infrastructure.Persistence.Repositories;

internal sealed class EventReadRepository(EventRepDbContext dbContext)
    : IEventReadRepository
{
    public Task<EventResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default) =>
        dbContext.Events
            .AsNoTracking()
            .Where(eventOrm => eventOrm.Id == id)
            .Select(eventOrm => new EventResponse(
                eventOrm.Id,
                eventOrm.Name,
                eventOrm.CustomerId,
                eventOrm.ExecutorId,
                eventOrm.Status,
                eventOrm.TimeStart,
                eventOrm.TimeEnd,
                eventOrm.FullDate,
                eventOrm.CreatedAt))
            .SingleOrDefaultAsync(cancellationToken);

    public Task<PagedResult<EventResponse>> GetPageAsync(
        PageParams pageParams,
        EventStatus? status = null,
        Guid? customerId = null,
        Guid? executorId = null,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Events.AsNoTracking();

        if (status.HasValue)
            query = query.Where(eventOrm => eventOrm.Status == status.Value);

        if (customerId.HasValue)
            query = query.Where(eventOrm => eventOrm.CustomerId == customerId.Value);

        if (executorId.HasValue)
            query = query.Where(eventOrm => eventOrm.ExecutorId == executorId.Value);

        return query
            .OrderBy(eventOrm => eventOrm.FullDate)
            .ThenBy(eventOrm => eventOrm.CreatedAt)
            .Select(eventOrm => new EventResponse(
                eventOrm.Id,
                eventOrm.Name,
                eventOrm.CustomerId,
                eventOrm.ExecutorId,
                eventOrm.Status,
                eventOrm.TimeStart,
                eventOrm.TimeEnd,
                eventOrm.FullDate,
                eventOrm.CreatedAt))
            .ToPagedAsync(pageParams, cancellationToken);
    }
}
