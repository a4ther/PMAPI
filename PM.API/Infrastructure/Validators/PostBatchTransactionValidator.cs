using FluentValidation;
using PM.API.Models.Request;

namespace PM.API.Infrastructure.Validators
{
    public class PostBatchTransactionValidator : AbstractValidator<PostBatchTransaction>
    {
        public PostBatchTransactionValidator()
        {
            RuleFor(t => t.Amount).NotEmpty();
            RuleFor(t => t.CategoryName).NotEmpty();
            RuleFor(t => t.Currency).NotEmpty().IsInEnum();
            RuleFor(t => t.Date).NotEmpty();
            RuleFor(t => t.Wallet).Length(0, 10);
        }
    }
}
