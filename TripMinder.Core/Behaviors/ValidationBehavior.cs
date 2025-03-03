using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using TripMinder.Core.Resources;

namespace TripMinder.Core.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> validators;
    private readonly IStringLocalizer<SharedResources> stringLocalizer;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators,
        IStringLocalizer<SharedResources> stringLocalizer)
    {
        this.validators = validators;
        this.stringLocalizer = stringLocalizer;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                var message = failures.Select(x => stringLocalizer[$"{x.PropertyName}"] + ":" + stringLocalizer[x.ErrorMessage]).FirstOrDefault();
                throw new ValidationException(message);
            }
        }
        return await next();
    }
}