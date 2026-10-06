using EventRep.Application.Contracts.Persistence;
using EventRep.Application.Events.Common;
using EventRep.Domain.Contracts.Pagination;
using MediatR;

namespace EventRep.Application.Events.Queries.GetEvents;

internal sealed class GetEventsQueryHandler(
    IEventReadRepository eventReadRepository)
    : IRequestHandler<GetEventsQuery, Result<PagedResult<EventResponse>>>
{
    public async Task<Result<PagedResult<EventResponse>>> Handle(
        GetEventsQuery query,
        CancellationToken cancellationToken)
    {
        var page = await eventReadRepository.GetPageAsync(
            query.PageParams,
            query.Status,
            query.CustomerId,
            query.ExecutorId,
            cancellationToken);

        return Result.Ok(page);
    }
}
