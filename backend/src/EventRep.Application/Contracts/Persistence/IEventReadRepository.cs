using EventRep.Application.Events.Common;
using EventRep.Domain.Contracts.Pagination;
using EventRep.Domain.Enums;

namespace EventRep.Application.Contracts.Persistence;

public interface IEventReadRepository
{
    Task<EventResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PagedResult<EventResponse>> GetPageAsync(
        PageParams pageParams,
        EventStatus? status = null,
        Guid? customerId = null,
        Guid? executorId = null,
        CancellationToken cancellationToken = default);
}
