using FluentValidation;

namespace DemoEventi.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.UpdateUserDto.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("First name can only contain letters and spaces");

        RuleFor(x => x.UpdateUserDto.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Last name can only contain letters and spaces");

        RuleFor(x => x.UpdateUserDto.InterestIds)
            .NotNull().WithMessage("Interest IDs cannot be null");
    }
}
