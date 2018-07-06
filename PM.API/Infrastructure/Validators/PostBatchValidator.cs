using FluentValidation;
using PM.API.Models.Request;

namespace PM.API.Infrastructure.Validators
{
    public class PostBatchValidator : AbstractValidator<PostBatch>
    {
        public PostBatchValidator()
        {
            RuleFor(b => b.AllowDuplicates).Must(x => x == false || x == true);
            RuleFor(b => b.ExcludeTransfers).Must(x => x == false || x == true);
            RuleFor(b => b.Transactions).NotEmpty().SetCollectionValidator(new PostBatchTransactionValidator());
        }
    }
}
