using Common;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Middlewares;

public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
 

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators
    )
    {
        _validators = validators;        
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var context = new ValidationContext<TRequest>(request);
        var name = typeof(TRequest).Name;

        var failures = _validators
          .Select(v => (v.Validate(context)))
          .SelectMany(result => result.Errors)
          .ToList();

        if (failures.Count != 0)
        {
            var message = $"HelpApp Long Running Request: {name} \n";
            foreach (var failure in failures)
            {
                message += $" {failure.ErrorMessage} \n";
            }

            var errors = failures.Select(f => f.ErrorMessage).ToArray();

            return CreateValidationResult<TResponse>(errors);
        }

        return await next();
    }

    private static TResponse CreateValidationResult<TResponse>(string[] errors)
        where TResponse : Result
    {
        if (typeof(TResponse) == typeof(Result))
        {
            return Result.Failure(StatusCode.BadRequest, errors) as TResponse ?? throw new InvalidOperationException("Unexpected cast failure");
        }

        var genericType = typeof(TResponse).GenericTypeArguments[0];
        var failureMethod = typeof(Result<>)
            .MakeGenericType(genericType)
            .GetMethod(nameof(Result.Failure)) ?? throw new InvalidOperationException($"Method {nameof(Result.Failure)} not found");

        var result = failureMethod.Invoke(null, new object?[] { StatusCode.BadRequest, errors }) ?? throw new InvalidOperationException($"Failed to invoke {nameof(Result.Failure)} method");

        return (TResponse)result;
    }
}