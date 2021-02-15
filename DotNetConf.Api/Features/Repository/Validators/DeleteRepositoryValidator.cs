using DotNetConf.Api.Features.Repository.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetConf.Api.Features.Repository.Validators
{
    public class DeleteRepositoryValidator : AbstractValidator<DeleteRepositoryCommand>
    {
        public DeleteRepositoryValidator()
        {
            RuleFor(m => m).NotEmpty().WithMessage("Boş model gönderilemez.");
            RuleFor(m => m).Must(m=>!string.IsNullOrEmpty(m.Username) || m.UserId.HasValue).WithMessage("Username veya UserId bilgisi boş geçilemez.");
            RuleFor(m => m).Must(m=>!string.IsNullOrEmpty(m.Name) || m.Id.HasValue).WithMessage("Name veya Id bilgisi boş geçilemez.");
        }
    }
}
