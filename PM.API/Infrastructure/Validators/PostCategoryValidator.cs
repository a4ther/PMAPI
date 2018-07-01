using FluentValidation;
using PM.API.Models.Request;

namespace PM.API.Infrastructure.Validators
{
    public class PostCategoryValidator : AbstractValidator<PostCategory>
    {
        public PostCategoryValidator()
        {
            RuleFor(c => c.Name).NotEmpty().Length(1, 25);
        }
    }
}
