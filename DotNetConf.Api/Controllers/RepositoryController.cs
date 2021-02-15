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
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetConf.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}")]
    public class RepositoryController : BaseController
    {
        private readonly IMediator _mediator;
        public RepositoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("users/{username}/repos/{name}")]
        [ProducesResponseType(typeof(BaseResponseModel<RepositoryModel>), 200)]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 404)]
        [ProducesResponseType(typeof(BaseResponseModel), 401)]
        [ProducesResponseType(typeof(BaseResponseModel), 422)]
        [SwaggerOperation(
            Summary = "Get Repository's information",
            Description = "Get Repository's information by username and name",
            Tags = new string[] { "Repository Get" })]
        public async Task<ActionResult<RepositoryModel>> Get(string username, string name)
        {
            return Ok(new BaseResponseModel<RepositoryModel>(await _mediator.Send(new GetRepositoryQuery(username, name))));
        }

        [HttpGet("users/{userId:long}/repos/{id:long}")]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 404)]
        [ProducesResponseType(typeof(BaseResponseModel), 401)]
        [ProducesResponseType(typeof(BaseResponseModel), 422)]
        [SwaggerOperation(
            Summary = "Get Repository's information",
            Description = "Get Repository's information by userId and id",
            Tags = new string[] { "Repository Get" })]
        public async Task<ActionResult<RepositoryModel>> Get(long userId, long id)
        {
            return Ok(new BaseResponseModel<RepositoryModel>(await _mediator.Send(new GetRepositoryQuery(userId, id))));
        }

        [HttpGet("users/{username}/repos")]
        [ProducesResponseType(typeof(BaseResponseModel<List<RepositoryModel>>), 200)]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 401)]
        [ProducesResponseType(typeof(BaseResponseModel), 422)]
        [SwaggerOperation(
            Summary = "Get Repository list",
            Description = "Get Repository list by username",
            Tags = new string[] { "Repository Get" })]
        public async Task<ActionResult<RepositoryModel>> GetList(string username)
        {
            return Ok(new BaseResponseModel<List<RepositoryModel>>(await _mediator.Send(new GetRepositoriesQuery(username))));
        }

        [HttpGet("users/{userId:long}/repos")]
        [ProducesResponseType(typeof(BaseResponseModel<List<RepositoryModel>>), 200)]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 401)]
        [ProducesResponseType(typeof(BaseResponseModel), 422)]
        [SwaggerOperation(
            Summary = "Get Repository list",
            Description = "Get Repository list by userId",
            Tags = new string[] { "Repository Get" })]
        public async Task<ActionResult<RepositoryModel>> GetList(long userId)
        {
            return Ok(new BaseResponseModel<List<RepositoryModel>>(await _mediator.Send(new GetRepositoriesQuery(userId))));
        }

        [HttpPost("repos")]
        [ProducesResponseType(typeof(BaseResponseModel<RepositoryModel>), 201)]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 401)]
        [ProducesResponseType(typeof(BaseResponseModel), 422)]
        [SwaggerOperation(
            Summary = "Create Repository",
            Description = "Create Repository description",
            Tags = new string[] { "Repository Post" })]
        public async Task<ActionResult<RepositoryModel>> Create([FromBody] CreateRepositoryCommand model)
        {
            return Created(string.Empty, await _mediator.Send(model));
        }

        [HttpPut("repos")]
        [ProducesResponseType(typeof(BaseResponseModel<RepositoryModel>), 201)]
        [ProducesResponseType(typeof(BaseResponseModel<RepositoryModel>), 200)]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 401)]
        [ProducesResponseType(typeof(BaseResponseModel), 422)]
        [SwaggerOperation(
            Summary = "Update Repository",
            Description = "Update Repository description",
            Tags = new string[] { "Repository Put" })]
        public async Task<ActionResult<RepositoryModel>> Update([FromBody] UpdateRepositoryCommand model)
        {
            var (repositoryModel, isCreated) = await _mediator.Send(model);
            return isCreated
                ? Created(string.Empty, new BaseResponseModel<RepositoryModel>(repositoryModel))
                : Ok(new BaseResponseModel<RepositoryModel>(repositoryModel));
        }

        [HttpDelete("users/{username}/repos/{name}")]
        [ProducesResponseType(typeof(BaseResponseModel), 202)]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 401)]
        [ProducesResponseType(typeof(BaseResponseModel), 404)]
        [SwaggerOperation(
            Summary = "Delete Repository",
            Description = "Delete Repository by username and name",
            Tags = new string[] { "Repository Delete" })]
        public async Task<ActionResult<UserModel>> Delete(string username, string name)
        {
            await _mediator.Send(new DeleteRepositoryCommand(username, name));
            return Accepted(new BaseResponseModel("Repository deleted successfully"));
        }

        [HttpDelete("users/{userId:long}/repos/{id:long}")]
        [ProducesResponseType(typeof(BaseResponseModel), 202)]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 401)]
        [ProducesResponseType(typeof(BaseResponseModel), 404)]
        [SwaggerOperation(
            Summary = "Delete Repository",
            Description = "Delete Repository by userId and id",
            Tags = new string[] { "Repository Delete" })]
        public async Task<ActionResult<UserModel>> Delete(long userId, long id)
        {
            await _mediator.Send(new DeleteRepositoryCommand(userId, id));
            return Accepted(new BaseResponseModel("Repository deleted successfully"));
        }

    }
}
