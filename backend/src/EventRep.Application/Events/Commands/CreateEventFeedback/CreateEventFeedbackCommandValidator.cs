using FluentValidation;

namespace EventRep.Application.Events.Commands.CreateEventFeedback;

internal sealed class CreateEventFeedbackCommandValidator
    : AbstractValidator<CreateEventFeedbackCommand>
{
    public CreateEventFeedbackCommandValidator()
    {
        RuleFor(command => command.EventId).NotEmpty();
        RuleFor(command => command.ExecutorId).NotEmpty();
    }
}
