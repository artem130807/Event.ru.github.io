using EventRep.Application.Contracts.Persistence;
using EventRep.Application.Events.Common;
using MediatR;

namespace EventRep.Application.Events.Queries.GetEventById;

internal sealed class GetEventByIdQueryHandler(
    IEventReadRepository eventReadRepository)
    : IRequestHandler<GetEventByIdQuery, Result<EventResponse>>
{
    public async Task<Result<EventResponse>> Handle(
        GetEventByIdQuery query,
        CancellationToken cancellationToken)
    {
        var eventResponse = await eventReadRepository.GetByIdAsync(
            query.Id,
            cancellationToken);

        return eventResponse is null
            ? Result.Fail("Событие не найдено.")
            : Result.Ok(eventResponse);
    }
}
