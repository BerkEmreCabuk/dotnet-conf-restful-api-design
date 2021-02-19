using DotNetConf.Api.Controllers.Base;
using DotNetConf.Api.Features.Repository.Commands;
using DotNetConf.Api.Features.Repository.Models;
using DotNetConf.Api.Features.Repository.Queries;
using DotNetConf.Api.Features.User.Models;
using DotNetConf.Api.Models.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNetConf.Api.Controllers.Version2
{
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/users")]
    public class RepositoryController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly LinkGenerator _linkGenerator;
        public RepositoryController(
            IMediator mediator,
            LinkGenerator linkGenerator)
        {
            _mediator = mediator;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("users/{userId:long}/repos/{id:long}", Name = nameof(GetById), Order = 1)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 422)]
        [SwaggerOperation(
            Summary = "Get Repository's information",
            Description = "Get Repository's information by userId and id")]
        public async Task<ActionResult<RepositoryModel>> GetById(long userId, long id)
        {
            return Ok(new BaseResponseModel<RepositoryModel>(await _mediator.Send(new GetRepositoryQuery(userId, id))));
        }

        [HttpGet("users/{userId:long}/repos", Name = nameof(GetListById), Order = 2)]
        [ProducesResponseType(typeof(BaseResponseModel<List<RepositoryModel>>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 422)]
        [SwaggerOperation(
            Summary = "Get Repository list",
            Description = "Get Repository list by userId")]
        public async Task<ActionResult<RepositoryModel>> GetListById(long userId)
        {
            return Ok(new BaseResponseModel<List<RepositoryModel>>(await _mediator.Send(new GetRepositoriesQuery(userId))));
        }

        [HttpDelete("users/{userId:long}/repos/{id:long}", Name = nameof(DeleteById), Order = 3)]
        [ProducesResponseType(typeof(BaseResponseModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [SwaggerOperation(
            Summary = "Delete Repository",
            Description = "Delete Repository by userId and id",
            Tags = new string[] { "Repository Delete" })]
        public async Task<ActionResult<UserModel>> DeleteById(long userId, long id)
        {
            await _mediator.Send(new DeleteRepositoryCommand(userId, id));
            return Ok(new BaseResponseModel("Repository deleted successfully"));
        }

        private BaseResponseModel CreateLinksForRepository(BaseResponseModel model)
        {
            string controllerName = (nameof(RepositoryController)).Replace("Controller", "");
            if (!HttpContext.Request.RouteValues.TryGetValue("version", out var version))
                version = ApiVersion.Default;

            if (!string.IsNullOrEmpty(_linkGenerator.GetPathByAction(HttpContext, nameof(GetById), controllerName, new { version = version.ToString() })))
            {
                model.Links.Add(
                new LinkModel
                {
                    Href = _linkGenerator.GetPathByAction(HttpContext, nameof(GetById), controllerName, new { version = version.ToString() }),
                    Method = HttpMethod.Get.ToString(),
                    Description = "Get Repository's information"
                });
            }
            model.Links.Add(
                new LinkModel
                {
                    Href = _linkGenerator.GetPathByAction(HttpContext, nameof(GetListById), controllerName, new { version = version.ToString() }),
                    Method = HttpMethod.Get.ToString(),
                    Description = "Get Repository list"
                });
            return model;
        }
    }
}
