using FluentValidation;
using MediatR;

namespace DemoEventi.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => !r.IsValid)
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                var errorMessage = string.Join("; ", failures.Select(f => f.ErrorMessage));

                // Return failure result using the existing Result pattern
                if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var resultType = typeof(TResponse).GetGenericArguments()[0];
                    var failureMethod = typeof(Result<>).MakeGenericType(resultType).GetMethod("Failure", new[] { typeof(string) });
                    return (TResponse)failureMethod!.Invoke(null, new object[] { $"Validation failed: {errorMessage}" })!;
                }
            }
        }

        return await next();
    }
}