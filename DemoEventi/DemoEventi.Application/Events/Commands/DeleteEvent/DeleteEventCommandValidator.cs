using FluentValidation;

namespace DemoEventi.Application.Events.Commands.DeleteEvent;

public class DeleteEventCommandValidator : AbstractValidator<DeleteEventCommand>
{
    public DeleteEventCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Event ID is required");
    }
}
