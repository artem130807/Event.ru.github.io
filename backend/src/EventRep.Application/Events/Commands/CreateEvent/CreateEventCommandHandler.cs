using EventRep.Domain.Contracts;
using MediatR;

namespace EventRep.Application.Events.Commands.CreateEvent;

internal sealed class CreateEventCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateEventCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateEventCommand command,
        CancellationToken cancellationToken)
    {
        var customer = await unitOfWork.Users.GetByIdAsync(
            command.CustomerId,
            cancellationToken);

        if (customer is null)
            return Result.Fail("Заказчик не найден.");

        var creationResult = EventEntity.Create(
            command.Name.Trim(),
            executorId: null,
            command.CustomerId,
            command.TimeStart,
            command.TimeEnd,
            command.FullDate);

        if (creationResult.IsFailed)
            return creationResult.ToResult<Guid>();

        await unitOfWork.Events.AddAsync(
            creationResult.Value,
            cancellationToken);

        return Result.Ok(creationResult.Value.Id);
    }
}
