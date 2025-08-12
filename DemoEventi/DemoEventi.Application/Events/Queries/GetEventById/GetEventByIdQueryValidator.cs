using FluentValidation;

namespace DemoEventi.Application.Events.Queries.GetEventById;

public class GetEventByIdQueryValidator : AbstractValidator<GetEventByIdQuery>
{
    public GetEventByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Event ID is required");
    }
}
