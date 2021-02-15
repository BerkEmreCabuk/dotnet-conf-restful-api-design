using AutoMapper;
using DotNetConf.Api.Entities;
using DotNetConf.Api.Enums;
using DotNetConf.Api.Features.User.Queries;
using DotNetConf.Api.Infrastructures.Database;
using DotNetConf.Api.Models.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetConf.Api.Features.User.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public DeleteUserCommand(string username)
        {
            Username = username;
        }

        public DeleteUserCommand(long id)
        {
            this.Id = id;
        }
        public string Username { get; private set; }
        public long? Id { get; private set; }
    }

    public class DeleteUserCommandHandler : AsyncRequestHandler<DeleteUserCommand>
    {
        private readonly IService _service;

        public DeleteUserCommandHandler(
            IService service)
        {
            _service = service;
        }

        protected override async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            UserEntity userEntity;

            if (!string.IsNullOrEmpty(request.Username))
            {
                userEntity = await _service.FindAsync<UserEntity>(x => x.Username == request.Username && x.Status == RecordStatuses.ACTIVE, true);
            }
            else if (request.Id.HasValue)
            {
                userEntity = await _service.FindAsync<UserEntity>(x => x.Id == request.Id && x.Status == RecordStatuses.ACTIVE, true);
            }
            else
            {
                throw new UnprocessableException();
            }

            if (userEntity == null)
                throw new NotFoundException();

            if (userEntity.Repositories != null)
            {
                foreach (var repo in userEntity.Repositories.Where(x => x.Status == RecordStatuses.ACTIVE))
                {
                    repo.Delete();
                }
            }

            _service.Delete(userEntity);
            await _service.SaveChangesAsync();
        }
    }
}
