using DotNetConf.Api.Models;
using DotNetConf.Api.Models.BaseModels;
using DotNetConf.Api.Models.Exceptions;
using DotNetConf.Api.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetConf.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/identities")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class IdentityController : Controller
    {
        private readonly IOptions<IdentitySettingModel> _settings;

        public IdentityController(
            IOptions<IdentitySettingModel> settings)
        {
            _settings = settings;
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponseModel<IdentityResponseModel>), 200)]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 404)]
        [SwaggerOperation(
            Summary = "Get Token",
            Description = "Get Token description",
            Tags = new string[] { "Auth" })]
        public ActionResult GetToken([FromBody]IdentityRequestModel model)
        {
            if (model.UserName != "user" || model.Password != "1234")
                throw new NotFoundException("Not Found User");

            var tokenHandler = new JwtSecurityTokenHandler();
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.Value.SecretKey));
            var jwt = new JwtSecurityToken(
                issuer: _settings.Value.Iss,
                audience: _settings.Value.Aud,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(new IdentityResponseModel(encodedJwt));
        }
    }
}
