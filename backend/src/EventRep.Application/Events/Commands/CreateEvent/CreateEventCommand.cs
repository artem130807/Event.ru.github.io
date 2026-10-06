using EventRep.Application.Common.Messaging;
using MediatR;

namespace EventRep.Application.Events.Commands.CreateEvent;

public sealed record CreateEventCommand(
    string Name,
    Guid CustomerId,
    TimeSpan TimeStart,
    TimeSpan TimeEnd,
    DateTime FullDate) : IRequest<Result<Guid>>, ICommandRequest;
