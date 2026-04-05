using FluentValidation;
using Shared.Mediator.Application;

namespace WeChooz.TechAssessment.Application.Behaviors;

public sealed class RequestValidationBehavior<TRequest, TResult>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    public async Task<TResult> HandleAsync(TRequest input, Func<Task<TResult>> next, CancellationToken cancellationToken = default)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(input);
            var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = results.SelectMany(r => r.Errors).ToList();
            if (failures.Count > 0)
                throw new ValidationException(failures);
        }

        return await next();
    }
}
