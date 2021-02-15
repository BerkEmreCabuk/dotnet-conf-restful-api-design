using DotNetConf.Api.Features.User.Commands;
using FluentValidation;

namespace DotNetConf.Api.Features.User.Validators
{
    public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserValidator()
        {
            RuleFor(m => m).NotEmpty().WithMessage("Boş model gönderilemez.");
            RuleFor(m => m.Username).NotEmpty().WithMessage("Kullanıcı Adı boş bırakılamaz(100 karakter sınırı vardır).").MaximumLength(100);
        }
    }
}
