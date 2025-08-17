using DemoEventi.Application.Common;
using DemoEventi.Domain.Interfaces;
using MediatR;

namespace DemoEventi.Application.Events.Commands.DeleteEvent;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEventCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var ev = await _unitOfWork.Events.GetByIdAsync(request.Id);
            if (ev == null)
            {
                return Result.Failure("Event not found");
            }

            _unitOfWork.Events.Delete(ev);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error deleting event: {ex.Message}");
        }
    }
}
