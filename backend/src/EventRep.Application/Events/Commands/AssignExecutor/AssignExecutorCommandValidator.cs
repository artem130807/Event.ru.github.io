using FluentValidation;

namespace EventRep.Application.Events.Commands.AssignExecutor;

internal sealed class AssignExecutorCommandValidator
    : AbstractValidator<AssignExecutorCommand>
{
    public AssignExecutorCommandValidator()
    {
        RuleFor(command => command.EventId).NotEmpty();
        RuleFor(command => command.ExecutorId).NotEmpty();
    }
}
