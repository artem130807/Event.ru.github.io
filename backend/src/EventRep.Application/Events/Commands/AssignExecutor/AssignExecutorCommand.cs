using EventRep.Application.Common.Messaging;
using MediatR;

namespace EventRep.Application.Events.Commands.AssignExecutor;

public sealed record AssignExecutorCommand(
    Guid EventId,
    Guid ExecutorId) : IRequest<Result<Guid>>, ITransactionalRequest;
