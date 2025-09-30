using Corporate.Cashflow.Application.Common;
using FluentValidation;
using MediatR;

namespace Corporate.Cashflow.Application.Middlewares
{

    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, Result<TResponse>>
        where TRequest : IRequest<Result<TResponse>>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<Result<TResponse>> Handle(
            TRequest request,
            RequestHandlerDelegate<Result<TResponse>> next,
            CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var failures = _validators
                    .Select(v => v.Validate(context))
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .Select(f => f.ErrorMessage)
                    .ToList();

                if (failures.Any())
                    return Result<TResponse>.Failure(failures);
            }

            return await next();
        }
    }
}


