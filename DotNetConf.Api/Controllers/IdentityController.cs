using System.Threading.Tasks;
using DotNetConf.Api.Features.Identity.Commands;
using DotNetConf.Api.Features.Identity.Models;
using DotNetConf.Api.Models.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

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
        private readonly LinkGenerator _linkGenerator;
        private readonly IMediator _mediator;

        public IdentityController(
            IOptions<IdentitySettingModel> settings, IMediator mediator, LinkGenerator linkGenerator)
        {
            _settings = settings;
            _mediator = mediator;
            _linkGenerator = linkGenerator;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponseModel<IdentityResponseModel>), 200)]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 404)]
        [SwaggerOperation(
            Summary = "Get Token",
            Description = "Get Token description",
            Tags = new[] {"Auth"})]
        public async Task<ActionResult<IdentityResponseModel>> GetToken([FromBody] CreateJwtTokenCommand model)
        {
            var response = new BaseResponseModel<IdentityResponseModel>(await _mediator.Send(model));
            return Created(string.Empty, CreateLinksForIdentity(response));
        }

        private BaseResponseModel CreateLinksForIdentity(BaseResponseModel model)
        {
            var controllerName =
                nameof(IdentityController)
                    .Replace("Controller", "");

            if (!HttpContext.Request.RouteValues.TryGetValue("version", out var version))
                version = ApiVersion.Default;

            model.Links.Add(
                new LinkModel
                {
                    Href = _linkGenerator.GetPathByAction(
                        HttpContext,
                        nameof(GetToken),
                        controllerName,
                        new {version = version?.ToString()}
                    ),
                    Method = HttpMethod.Get.ToString(),
                    Description = "Get Identity's information"
                });
            return model;
        }
    }
}