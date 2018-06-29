using FluentValidation;
using PM.API.Models;

namespace PM.API.Infrastructure.Validators
{
	public class PostTransactionValidator : AbstractValidator<PostTransaction>
    {
        public PostTransactionValidator()
        {
            RuleFor(t => t.Amount).NotEmpty();
            RuleFor(t => t.CategoryID).NotEmpty();
            RuleFor(t => t.Currency).NotEmpty().IsInEnum();
            RuleFor(t => t.Date).NotEmpty();
            RuleFor(t => t.Wallet).Length(0, 10);
        }
    }
}
