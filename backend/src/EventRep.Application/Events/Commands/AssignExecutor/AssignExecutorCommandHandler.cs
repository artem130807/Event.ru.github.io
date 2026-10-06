using EventRep.Domain.Contracts;
using MediatR;

namespace EventRep.Application.Events.Commands.AssignExecutor;

internal sealed class AssignExecutorCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AssignExecutorCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        AssignExecutorCommand command,
        CancellationToken cancellationToken)
    {
        var eventEntity = await unitOfWork.Events.GetByIdAsync(
            command.EventId,
            cancellationToken);

        if (eventEntity is null)
            return Result.Fail("Событие не найдено.");

        var hasFeedback = await unitOfWork.EventFeedbacks.ExistsAsync(
            command.EventId,
            command.ExecutorId,
            cancellationToken);

        if (!hasFeedback)
            return Result.Fail("Этот исполнитель не откликался на событие.");

        var assignmentResult = eventEntity.AssignExecutor(command.ExecutorId);
        if (assignmentResult.IsFailed)
            return assignmentResult.ToResult<Guid>();

        unitOfWork.Events.Update(eventEntity);

        return Result.Ok(eventEntity.Id);
    }
}
