using DotNetConf.Api.Controllers.Base;
using DotNetConf.Api.Features.User.Commands;
using DotNetConf.Api.Features.User.Models;
using DotNetConf.Api.Features.User.Queries;
using DotNetConf.Api.Models.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNetConf.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/users")]
    public class UserController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly LinkGenerator _linkGenerator;
        public UserController(
            IMediator mediator,
            LinkGenerator linkGenerator)
        {
            _mediator = mediator;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("{username}", Name = nameof(UserGetByUsername))]
        [ProducesResponseType(typeof(BaseResponseModel<UserModel>), 200)]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 404)]
        [ProducesResponseType(typeof(BaseResponseModel), 401)]
        [ProducesResponseType(typeof(BaseResponseModel), 422)]
        [SwaggerOperation(
            Summary = "Get User's information",
            Description = "Get User's information description",
            Tags = new string[] { "User Get" })]
        public async Task<ActionResult<UserModel>> UserGetByUsername(string username)
        {
            var response = new BaseResponseModel<UserModel>(await _mediator.Send(new GetUserQuery(username)));
            return Ok(CreateLinksForUser(response));
        }

        [HttpGet(Name = nameof(UserGetList))]
        [ProducesResponseType(typeof(BaseResponseModel<List<UserModel>>), 200)]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 404)]
        [ProducesResponseType(typeof(BaseResponseModel), 401)]
        [SwaggerOperation(
            Summary = "Get User List",
            Description = "Get User list description",
            Tags = new string[] { "User Get" })]
        public async Task<ActionResult<UserModel>> UserGetList()
        {
            return Ok(new BaseResponseModel<List<UserModel>>(await _mediator.Send(new GetUsersQuery())));
        }

        [HttpPost(Name = nameof(UserCreate))]
        [ProducesResponseType(typeof(BaseResponseModel<UserModel>), 201)]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 401)]
        [ProducesResponseType(typeof(BaseResponseModel), 422)]
        [SwaggerOperation(
            Summary = "Create User",
            Description = "Create User description",
            Tags = new string[] { "User Post" })]
        public async Task<ActionResult<UserModel>> UserCreate([FromBody] CreateUserCommand model)
        {
            var response = new BaseResponseModel<UserModel>(await _mediator.Send(model));
            return Created(string.Empty, CreateLinksForUser(response));
        }

        [HttpPut(Name = nameof(UserUpdate))]
        [ProducesResponseType(typeof(BaseResponseModel<UserModel>), 201)]
        [ProducesResponseType(typeof(BaseResponseModel<UserModel>), 200)]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 401)]
        [ProducesResponseType(typeof(BaseResponseModel), 422)]
        [SwaggerOperation(
            Summary = "Update User",
            Description = "Update User description",
            Tags = new string[] { "User Put" })]
        public async Task<ActionResult<UserModel>> UserUpdate([FromBody] UpdateUserCommand model)
        {
            var (userModel, isCreated) = await _mediator.Send(model);
            return isCreated
                ? Created(string.Empty, CreateLinksForUser(new BaseResponseModel<UserModel>(userModel)))
                : Ok(CreateLinksForUser(new BaseResponseModel<UserModel>(userModel)));
        }

        [HttpDelete("{username}", Name = nameof(UserDeleteByUsername))]
        [ProducesResponseType(typeof(BaseResponseModel), 202)]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 401)]
        [ProducesResponseType(typeof(BaseResponseModel), 404)]
        [SwaggerOperation(
            Summary = "Delete User",
            Description = "Delete User description",
            Tags = new string[] { "User Delete" })]
        public async Task<ActionResult<UserModel>> UserDeleteByUsername(string username)
        {
            await _mediator.Send(new DeleteUserCommand(username));
            return Ok(CreateLinksForUser(new BaseResponseModel("User deleted successfully.")));
        }

        private BaseResponseModel CreateLinksForUser(BaseResponseModel model)
        {
            string controllerName = (nameof(UserController)).Replace("Controller", "");
            if (!HttpContext.Request.RouteValues.TryGetValue("version", out var version))
                version = ApiVersion.Default;

            if (!string.IsNullOrEmpty(_linkGenerator.GetPathByAction(HttpContext, nameof(UserGetByUsername), controllerName, new { version = version.ToString() })))
            {
                model.Links.Add(
                new LinkModel
                {
                    Href = _linkGenerator.GetPathByAction(HttpContext, nameof(UserGetByUsername), controllerName, new { version = version.ToString() }),
                    Method = HttpMethod.Get.ToString(),
                    Description = "Get User's information"
                });
            }
            model.Links.Add(
                new LinkModel
                {
                    Href = _linkGenerator.GetPathByAction(HttpContext, nameof(UserGetList), controllerName, new { version = version.ToString() }),
                    Method = HttpMethod.Get.ToString(),
                    Description = "Get User list"
                });
            model.Links.Add(
            new LinkModel
            {
                Href = _linkGenerator.GetPathByAction(HttpContext, nameof(UserCreate), controllerName, new { version = version.ToString() }),
                Method = HttpMethod.Post.ToString(),
                Description = "Create User"
            });
            model.Links.Add(
            new LinkModel
            {
                Href = _linkGenerator.GetPathByAction(HttpContext, nameof(UserUpdate), controllerName, new { version = version.ToString() }),
                Method = HttpMethod.Put.ToString(),
                Description = "Update User"
            });
            return model;
        }
    }
}
