using FluentValidation;

namespace EventRep.Application.Events.Commands.CreateEvent;

internal sealed class CreateEventCommandValidator
    : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.CustomerId)
            .NotEmpty();

        RuleFor(command => command.TimeStart)
            .LessThan(command => command.TimeEnd)
            .WithMessage("Время начала должно быть раньше времени окончания.");

        RuleFor(command => command.FullDate)
            .NotEmpty();
    }
}
