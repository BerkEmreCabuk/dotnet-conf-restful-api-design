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

namespace DotNetConf.Api.Controllers.Version2
{
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/users")]
    public class UserController : BaseController
    {
        private readonly IMediator _mediator;
        public UserController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:long}", Name = nameof(GetById))]
        [ProducesResponseType(typeof(BaseResponseModel<UserModel>), 200)]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 404)]
        [ProducesResponseType(typeof(BaseResponseModel), 401)]
        [ProducesResponseType(typeof(BaseResponseModel), 422)]
        [SwaggerOperation(
            Summary = "Get User",
            Description = "Get User's information by id description",
            Tags = new string[] { "User Get" })]
        public async Task<ActionResult<UserModel>> GetById(long id)
        {
            var response = new BaseResponseModel<UserModel>(await _mediator.Send(new GetUserQuery(id)));
            return Ok(response);
        }

        [HttpDelete("{id:long}", Name = nameof(DeleteById))]
        [ProducesResponseType(typeof(BaseResponseModel), 202)]
        [ProducesResponseType(typeof(BaseResponseModel), 400)]
        [ProducesResponseType(typeof(BaseResponseModel), 401)]
        [ProducesResponseType(typeof(BaseResponseModel), 404)]
        [SwaggerOperation(
            Summary = "Delete User",
            Description = "Delete User by id description",
            Tags = new string[] { "User Delete" })]
        public async Task<ActionResult<UserModel>> DeleteById(long id)
        {
            await _mediator.Send(new DeleteUserCommand(id));
            return Accepted(new BaseResponseModel("User deleted successfully"));
        }
    }
}
