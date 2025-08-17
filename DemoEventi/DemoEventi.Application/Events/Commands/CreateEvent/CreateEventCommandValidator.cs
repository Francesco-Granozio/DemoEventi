using FluentValidation;

namespace DemoEventi.Application.Events.Commands.CreateEvent;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.CreateEventDto.Name)
            .NotEmpty().WithMessage("Event name is required")
            .MaximumLength(100).WithMessage("Event name cannot exceed 100 characters")
            .Matches(@"^[a-zA-Z0-9\s\-_]+$").WithMessage("Event name can only contain letters, numbers, spaces, hyphens, and underscores");

        RuleFor(x => x.CreateEventDto.Location)
            .NotEmpty().WithMessage("Location is required")
            .MaximumLength(200).WithMessage("Location cannot exceed 200 characters");

        RuleFor(x => x.CreateEventDto.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .GreaterThan(DateTime.Now).WithMessage("Start date must be in the future");

        RuleFor(x => x.CreateEventDto.ParticipantIds)
            .NotNull().WithMessage("Participant IDs cannot be null");
    }
}
