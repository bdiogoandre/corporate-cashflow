using FluentValidation;

namespace Corporate.Cashflow.Application.UseCases.Accounts.Create
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.");

            RuleFor(x => x.Currency)
                .IsInEnum()
                .WithMessage("Currency must be a valid value.");
        }
    }
}
