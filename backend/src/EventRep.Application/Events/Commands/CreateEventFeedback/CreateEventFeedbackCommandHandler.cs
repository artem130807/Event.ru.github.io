using EventRep.Domain.Contracts;
using EventRep.Domain.Entity;
using MediatR;

namespace EventRep.Application.Events.Commands.CreateEventFeedback;

internal sealed class CreateEventFeedbackCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateEventFeedbackCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateEventFeedbackCommand command,
        CancellationToken cancellationToken)
    {
        var eventEntity = await unitOfWork.Events.GetByIdAsync(
            command.EventId,
            cancellationToken);

        if (eventEntity is null)
            return Result.Fail("Событие не найдено.");

        var executor = await unitOfWork.Users.GetByIdAsync(
            command.ExecutorId,
            cancellationToken);

        if (executor is null)
            return Result.Fail("Исполнитель не найден.");

        if (eventEntity.CustomerId == command.ExecutorId)
            return Result.Fail("Заказчик не может откликнуться на собственное событие.");

        if (await unitOfWork.EventFeedbacks.ExistsAsync(
                command.EventId,
                command.ExecutorId,
                cancellationToken))
        {
            return Result.Fail("Исполнитель уже откликнулся на это событие.");
        }

        var creationResult = EventFeedbackEntity.Create(
            command.EventId,
            command.ExecutorId);

        if (creationResult.IsFailed)
            return creationResult.ToResult<Guid>();

        await unitOfWork.EventFeedbacks.AddAsync(
            creationResult.Value,
            cancellationToken);

        return Result.Ok(creationResult.Value.Id);
    }
}
