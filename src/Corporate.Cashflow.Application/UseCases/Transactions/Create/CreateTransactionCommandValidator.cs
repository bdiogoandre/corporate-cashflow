using FluentValidation;

namespace Corporate.Cashflow.Application.UseCases.Transactions.Create
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(250)
                .WithMessage("Description cannot exceed 250 characters.");

            RuleFor(x => x.Date)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Date cannot be in the future.");

            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage("AccountId is required.");

            RuleFor(x => x.TransactionType)
                .IsInEnum()
                .WithMessage("TransactionType must be a valid value.");
        }
    }

}
