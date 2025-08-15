using FluentValidation;
using MediatR;

namespace HabitTracker.Application.Common.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest>[] _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators?.ToArray() ?? Array.Empty<IValidator<TRequest>>();
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (_validators.Length > 0)
            {
                var context = new ValidationContext<TRequest>(request);

                var results = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = results
                    .SelectMany(r => r.Errors)
                    .Where(e => e is not null)
                    .ToList();

                if (failures.Count > 0)
                    throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
