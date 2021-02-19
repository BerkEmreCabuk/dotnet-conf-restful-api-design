using DotNetConf.Api.Features.Repository.Commands;
using FluentValidation;

namespace DotNetConf.Api.Features.Repository.Validators
{
    public class ChangeVisibilityRepositoryValidator : AbstractValidator<ChangeVisibilityRepositoryCommand>
    {
        public ChangeVisibilityRepositoryValidator()
        {
            RuleFor(m => m).NotEmpty().WithMessage("Boş model gönderilemez.");
        }
    }
}
