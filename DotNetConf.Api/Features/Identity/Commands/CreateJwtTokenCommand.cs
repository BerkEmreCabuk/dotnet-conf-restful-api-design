using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetConf.Api.Features.Identity.Models;
using DotNetConf.Api.Models.Exceptions;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DotNetConf.Api.Features.Identity.Commands
{
    public class CreateJwtTokenCommand : IRequest<IdentityResponseModel>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class CreateJwtTokenCommandHandler : IRequestHandler<CreateJwtTokenCommand, IdentityResponseModel>
    {
        private readonly IOptions<IdentitySettingModel> _settings;

        public CreateJwtTokenCommandHandler(IOptions<IdentitySettingModel> settings)
        {
            _settings = settings;
        }

        public async Task<IdentityResponseModel> Handle(CreateJwtTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserName) &&
                string.IsNullOrEmpty(request.Password))
            {
                throw new UnprocessableException("Username and password cannot be empty");
            }

            var tokenHandler = 
                new JwtSecurityTokenHandler();

            var signingKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_settings.Value.SecretKey)
            );

            var jwt = tokenHandler.CreateJwtSecurityToken
            (
                issuer: _settings.Value.Iss,
                audience: _settings.Value.Aud,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: 
                new SigningCredentials(signingKey, 
                    SecurityAlgorithms.HmacSha256)
            );
            
            var encodedJwt = 
                new JwtSecurityTokenHandler()
                    .WriteToken(jwt);
            
            return new IdentityResponseModel(encodedJwt);
        }
    }
}