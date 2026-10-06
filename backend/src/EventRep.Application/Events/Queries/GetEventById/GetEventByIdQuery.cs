using EventRep.Application.Events.Common;
using MediatR;

namespace EventRep.Application.Events.Queries.GetEventById;

public sealed record GetEventByIdQuery(Guid Id)
    : IRequest<Result<EventResponse>>;
