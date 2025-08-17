using DemoEventi.Domain.Interfaces;
using MediatR;

namespace DemoEventi.Application.Common.Behaviors;

public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Only apply transaction for commands (mutations), not queries
        var isCommand = typeof(TRequest).Name.EndsWith("Command");

        if (!isCommand)
        {
            return await next();
        }

        // Start transaction for commands
        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var response = await next();

            // Only commit if the result indicates success
            if (IsSuccessResult(response))
            {
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            else
            {
                await transaction.RollbackAsync();
            }

            return response;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private static bool IsSuccessResult(TResponse response)
    {
        // Check if it's a Result<T> and if it's successful
        if (response?.GetType().IsGenericType == true &&
            response.GetType().GetGenericTypeDefinition() == typeof(Result<>))
        {
            var isSuccessProperty = response.GetType().GetProperty("IsSuccess");
            return (bool)(isSuccessProperty?.GetValue(response) ?? false);
        }

        // For non-Result types, assume success
        return true;
    }
}
