using MediatR;
using DemoEventi.Application.Common;
using DemoEventi.Domain.Interfaces;

namespace DemoEventi.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            if (user == null)
            {
                return Result.Failure("User not found");
            }

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error deleting user: {ex.Message}");
        }
    }
}
