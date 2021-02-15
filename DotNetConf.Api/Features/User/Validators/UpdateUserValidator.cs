using DotNetConf.Api.Features.User.Commands;
using FluentValidation;

namespace DotNetConf.Api.Features.User.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(m => m).NotEmpty().WithMessage("Boş model gönderilemez.");
            RuleFor(m => m.Name).NotEmpty().WithMessage("İsim boş bırakılamaz(100 karakter sınırı vardır).").MaximumLength(100);
            RuleFor(m => m.Surname).NotEmpty().WithMessage("Soyad boş bırakılamaz(100 karakter sınırı vardır).").MaximumLength(100);
            RuleFor(m => m.Username).NotEmpty().WithMessage("Kullanıcı Adı boş bırakılamaz(100 karakter sınırı vardır).").MaximumLength(100);
            RuleFor(m => m.Email).NotEmpty().WithMessage("Email boş bırakılamaz(100 karakter sınırı vardır).");
        }
    }
}
