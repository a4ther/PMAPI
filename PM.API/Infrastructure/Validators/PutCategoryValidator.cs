using FluentValidation;
using PM.API.Models.Request;

namespace PM.API.Infrastructure.Validators
{
    public class PutCategoryValidator : AbstractValidator<PutCategory>
    {
        public PutCategoryValidator()
        {
            RuleFor(c => c.ID).NotEmpty();
            RuleFor(c => c.Name).NotEmpty().Length(1, 25);
        }
    }
}
