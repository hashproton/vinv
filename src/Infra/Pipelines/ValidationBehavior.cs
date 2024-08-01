using Application.Shared;
using FluentValidation;
using Infra.Errors;
using MediatR;
using System;

namespace Infra.Pipelines
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                var errorType = ValidationError.Create(failures);

                if (typeof(TResponse) == typeof(Result))
                {
                    var result = Result.Failure(errorType);
                    return result as TResponse ?? throw new InvalidOperationException("Failed to create failure Result.");
                }
                else if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var genericArgType = typeof(TResponse).GetGenericArguments()[0];
                    var failureResultMethod = typeof(Result<>)
                        .MakeGenericType(genericArgType)
                        .GetMethod(nameof(Result<object>.Failure), new[] { typeof(Error) });

                    if (failureResultMethod != null)
                    {
                        var failureResult = failureResultMethod.Invoke(null, new object[] { errorType });
                        return failureResult as TResponse ?? throw new InvalidOperationException("Failed to create failure Result<T>.");
                    }
                    else
                    {
                        throw new InvalidOperationException("Failed to find Failure method on Result<T>.");
                    }
                }
                else
                {
                    throw new ValidationException(failures);
                }
            }

            return await next();
        }
    }
}