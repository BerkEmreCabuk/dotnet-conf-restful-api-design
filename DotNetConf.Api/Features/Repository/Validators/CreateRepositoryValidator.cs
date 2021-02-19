using DotNetConf.Api.Features.Repository.Commands;
using FluentValidation;

namespace DotNetConf.Api.Features.Repository.Validators
{
    public class CreateRepositoryValidator : AbstractValidator<CreateRepositoryCommand>
    {
        public CreateRepositoryValidator()
        {
            RuleFor(m => m).NotEmpty().WithMessage("Boş model gönderilemez.");
            RuleFor(x => x).Must(x => !string.IsNullOrEmpty(x.Username) || x.UserId > 0).WithMessage("Username yada UserId bilgisi boş geçilemez");
            RuleFor(m => m.Name).NotEmpty().WithMessage("Name boş bırakılamaz(100 karakter sınırı vardır).").MaximumLength(100);
        }
    }
}
