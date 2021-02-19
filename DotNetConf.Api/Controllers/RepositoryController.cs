using DotNetConf.Api.Controllers.Base;
using DotNetConf.Api.Features.Repository.Commands;
using DotNetConf.Api.Features.Repository.Models;
using DotNetConf.Api.Features.Repository.Queries;
using DotNetConf.Api.Features.User.Commands;
using DotNetConf.Api.Features.User.Models;
using DotNetConf.Api.Features.User.Queries;
using DotNetConf.Api.Models.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNetConf.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}")]
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

        [HttpGet("users/{username}/repos/{name}", Name = nameof(GetByName))]
        [ProducesResponseType(typeof(BaseResponseModel<RepositoryModel>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 422)]
        [SwaggerOperation(
            Summary = "Get Repository's information",
            Description = "Get Repository's information by username and name",
            Tags = new string[] { "Repository Get" })]
        public async Task<ActionResult<RepositoryModel>> GetByName(string username, string name)
        {
            var response = new BaseResponseModel<RepositoryModel>(await _mediator.Send(new GetRepositoryQuery(username, name)));
            return Ok(CreateLinksForRepository(response));
        }

        [HttpGet("users/{username}/repos", Name = nameof(GetListByName))]
        [ProducesResponseType(typeof(BaseResponseModel<List<RepositoryModel>>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 422)]
        [SwaggerOperation(
            Summary = "Get Repository list",
            Description = "Get Repository list by username",
            Tags = new string[] { "Repository Get" })]
        public async Task<ActionResult<RepositoryModel>> GetListByName(string username)
        {
            var response = new BaseResponseModel<List<RepositoryModel>>(await _mediator.Send(new GetRepositoriesQuery(username)));
            return Ok(CreateLinksForRepository(response));
        }

        [HttpPost("repos", Name = nameof(Create))]
        [ProducesResponseType(typeof(BaseResponseModel<RepositoryModel>), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 422)]
        [SwaggerOperation(
            Summary = "Create Repository",
            Description = "Create Repository description",
            Tags = new string[] { "Repository Post" })]
        public async Task<ActionResult<RepositoryModel>> Create([FromBody] CreateRepositoryCommand model)
        {
            var response = new BaseResponseModel<RepositoryModel>(await _mediator.Send(model));
            return Created(string.Empty, CreateLinksForRepository(response));
        }

        [HttpPut("repos", Name = nameof(Update))]
        [ProducesResponseType(typeof(BaseResponseModel<RepositoryModel>), 201)]
        [ProducesResponseType(typeof(BaseResponseModel<RepositoryModel>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 422)]
        [SwaggerOperation(
            Summary = "Update Repository",
            Description = "Update Repository description",
            Tags = new string[] { "Repository Put" })]
        public async Task<ActionResult<RepositoryModel>> Update([FromBody] UpdateRepositoryCommand model)
        {
            var (repositoryModel, isCreated) = await _mediator.Send(model);
            return isCreated
                ? Created(string.Empty, CreateLinksForRepository(new BaseResponseModel<RepositoryModel>(repositoryModel)))
                : Ok(CreateLinksForRepository(new BaseResponseModel<RepositoryModel>(repositoryModel)));
        }

        [HttpPatch("users/{username}/repos/{name}/visibility", Name = nameof(ChangeVisibility))]
        [ProducesResponseType(typeof(BaseResponseModel<RepositoryModel>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 422)]
        [SwaggerOperation(
            Summary = "Change Repository's visibility",
            Description = "Change Repository's visibility description",
            Tags = new string[] { "Repository Patch" })]
        public async Task<ActionResult<RepositoryModel>> ChangeVisibility(string username, string name, [FromBody] ChangeVisibilityRepositoryCommand model)
        {
            model.Username = username;
            model.Name = name;
            var response = new BaseResponseModel<RepositoryModel>(await _mediator.Send(model));
            return Ok(CreateLinksForRepository(response));
        }

        [HttpDelete("users/{username}/repos/{name}", Name = nameof(DeleteByName))]
        [ProducesResponseType(typeof(BaseResponseModel), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 401)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [SwaggerOperation(
            Summary = "Delete Repository",
            Description = "Delete Repository by username and name",
            Tags = new string[] { "Repository Delete" })]
        public async Task<ActionResult<UserModel>> DeleteByName(string username, string name)
        {
            await _mediator.Send(new DeleteRepositoryCommand(username, name));
            return Ok(new BaseResponseModel("Repository deleted successfully"));
        }

        private BaseResponseModel CreateLinksForRepository(BaseResponseModel model)
        {
            string controllerName = (nameof(RepositoryController)).Replace("Controller", "");
            if (!HttpContext.Request.RouteValues.TryGetValue("version", out var version))
                version = ApiVersion.Default;

            if (!string.IsNullOrEmpty(_linkGenerator.GetPathByAction(HttpContext, nameof(GetByName), controllerName, new { version = version.ToString() })))
            {
                model.Links.Add(
                new LinkModel
                {
                    Href = _linkGenerator.GetPathByAction(HttpContext, nameof(GetByName), controllerName, new { version = version.ToString() }),
                    Method = HttpMethod.Get.ToString(),
                    Description = "Get Repository's information"
                });
            }
            model.Links.Add(
                new LinkModel
                {
                    Href = _linkGenerator.GetPathByAction(HttpContext, nameof(GetListByName), controllerName, new { version = version.ToString() }),
                    Method = HttpMethod.Get.ToString(),
                    Description = "Get Repository list"
                });
            model.Links.Add(
            new LinkModel
            {
                Href = _linkGenerator.GetPathByAction(HttpContext, nameof(Create), controllerName, new { version = version.ToString() }),
                Method = HttpMethod.Post.ToString(),
                Description = "Create Repository"
            });
            model.Links.Add(
            new LinkModel
            {
                Href = _linkGenerator.GetPathByAction(HttpContext, nameof(Update), controllerName, new { version = version.ToString() }),
                Method = HttpMethod.Put.ToString(),
                Description = "Update Repository"
            });
            return model;
        }
    }
}
