using EventRep.Application.Common.Messaging;
using MediatR;

namespace EventRep.Application.Events.Commands.CreateEventFeedback;

public sealed record CreateEventFeedbackCommand(
    Guid EventId,
    Guid ExecutorId) : IRequest<Result<Guid>>, ITransactionalRequest;
