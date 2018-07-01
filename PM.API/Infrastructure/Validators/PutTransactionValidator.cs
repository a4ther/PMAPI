using FluentValidation;
using PM.API.Models.Request;

namespace PM.API.Infrastructure.Validators
{
	public class PutTransactionValidator : AbstractValidator<PutTransaction>
    {
        public PutTransactionValidator()
        {
            RuleFor(t => t.ID).NotEmpty();
            RuleFor(t => t.Amount).NotEmpty();
            RuleFor(t => t.CategoryID).NotEmpty();
            RuleFor(t => t.Currency).NotEmpty().IsInEnum();
            RuleFor(t => t.Date).NotEmpty();
            RuleFor(t => t.Wallet).Length(0, 10);
        }
    }
}
