using Corporate.Cashflow.Application.Common;
using ErrorOr;
using FluentValidation;
using MediatR;

namespace Corporate.Cashflow.Application.Middlewares
{

    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, ErrorOr<TResponse>>
        where TRequest : IRequest<ErrorOr<TResponse>>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<ErrorOr<TResponse>> Handle(
            TRequest request,
            RequestHandlerDelegate<ErrorOr<TResponse>> next,
            CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var failures = _validators
                    .Select(v => v.Validate(context))
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .Select(f => f.ErrorMessage);

                if (failures.Any())
                    return Error.Validation(string.Join(';', failures));
            }

            return await next(cancellationToken);
        }
    }
}


