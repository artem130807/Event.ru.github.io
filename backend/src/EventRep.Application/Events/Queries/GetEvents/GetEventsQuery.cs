using EventRep.Application.Events.Common;
using EventRep.Domain.Contracts.Pagination;
using EventRep.Domain.Enums;
using MediatR;

namespace EventRep.Application.Events.Queries.GetEvents;

public sealed record GetEventsQuery(
    PageParams PageParams,
    EventStatus? Status = null,
    Guid? CustomerId = null,
    Guid? ExecutorId = null) : IRequest<Result<PagedResult<EventResponse>>>;
