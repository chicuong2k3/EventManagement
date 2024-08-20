

using EventManagement.Common.Application.Messaging;
using EventManagement.Common.Domain;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EventManagement.Common.Application.Behaviours
{
    internal sealed class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommandBase
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                validators.Select(x => x.ValidateAsync(context)));

            var validationFailures = validationResults
                .Where(x => !x.IsValid)
                .SelectMany(x => x.Errors)
                .ToArray();

            if (!validationFailures.Any())
            {
                return await next();
            }

            if (typeof(TResponse).IsGenericType 
                && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultType = typeof(TResponse).GetGenericArguments()[0];
                var failureMethod = typeof(Result<>)
                    .MakeGenericType(resultType)
                    .GetMethod(nameof(Result<object>.ValidationFailure));

                if (failureMethod != null)
                {
                    return (TResponse)failureMethod.Invoke(null, [CreateValidationError(validationFailures)])!;
                }
            }
            else if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.Failure(CreateValidationError(validationFailures));
            }

            throw new ValidationException(validationFailures);

        }

        static ValidationError CreateValidationError(ValidationFailure[] validationFailures)
        {
            return new ValidationError(
                validationFailures
                .Select(x => Error.Problem(x.ErrorCode, x.ErrorMessage))
                .ToArray()
            );
        }
    }
}
