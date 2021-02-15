using AutoMapper;
using DotNetConf.Api.Entities;
using DotNetConf.Api.Enums;
using DotNetConf.Api.Infrastructures.Database;
using DotNetConf.Api.Models.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetConf.Api.Features.Repository.Commands
{
    public class DeleteRepositoryCommand : IRequest
    {
        public DeleteRepositoryCommand(string username, string name)
        {
            this.Username = username;
            this.Name = name;
        }

        public DeleteRepositoryCommand(long id, long userId)
        {
            this.Id = id;
            this.UserId = userId;
        }
        public string Username { get; private set; }
        public string Name { get; private set; }
        public long? UserId { get; private set; }
        public long? Id { get; private set; }
    }

    public class DeleteRepositoryCommandHandler : AsyncRequestHandler<DeleteRepositoryCommand>
    {
        private readonly IService _service;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public DeleteRepositoryCommandHandler(
            IService service,
            IMapper mapper,
            IMediator mediator)
        {
            _service = service;
            _mapper = mapper;
            _mediator = mediator;
        }

        protected override async Task Handle(DeleteRepositoryCommand request, CancellationToken cancellationToken)
        {
            RepositoryEntity repositoryEntity;

            if (!string.IsNullOrEmpty(request.Username) && !string.IsNullOrEmpty(request.Name))
            {
                repositoryEntity = await _service.FindAsync<RepositoryEntity>(
                    x =>
                    x.User.Name == request.Username &&
                    x.Name == request.Name &&
                    x.Status == RecordStatuses.ACTIVE, true);
            }
            else if (request.Id.HasValue && request.UserId.HasValue)
            {
                repositoryEntity = await _service.FindAsync<RepositoryEntity>(
                    x =>
                    x.UserId == request.UserId.Value &&
                    x.Id == request.Id.Value &&
                    x.Status == RecordStatuses.ACTIVE, true);
            }
            else
            {
                throw new UnprocessableException();
            }

            if (repositoryEntity == null)
                throw new NotFoundException();

            _service.Delete(repositoryEntity);
            await _service.SaveChangesAsync();
        }
    }
}
