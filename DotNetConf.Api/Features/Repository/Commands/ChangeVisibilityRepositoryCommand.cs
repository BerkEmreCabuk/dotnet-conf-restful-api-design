using AutoMapper;
using DotNetConf.Api.Entities;
using DotNetConf.Api.Enums;
using DotNetConf.Api.Features.Repository.Models;
using DotNetConf.Api.Features.User.Queries;
using DotNetConf.Api.Infrastructures.Database;
using DotNetConf.Api.Infrastructures.Mapper;
using DotNetConf.Api.Models.Exceptions;
using MediatR;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetConf.Api.Features.Repository.Commands
{
    public class ChangeVisibilityRepositoryCommand : IRequest<RepositoryModel>
    {
        public bool IsPrivate { get; set; }
        [JsonIgnore]
        public string Username { get; set; }
        [JsonIgnore]
        public string Name { get; set; }
    }

    public class ChangeVisibilityRepositoryCommandHandler : IRequestHandler<ChangeVisibilityRepositoryCommand, RepositoryModel>
    {
        private readonly IService _service;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ChangeVisibilityRepositoryCommandHandler(
            IService service,
            IMapper mapper,
            IMediator mediator)
        {
            _service = service;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<RepositoryModel> Handle(ChangeVisibilityRepositoryCommand request, CancellationToken cancellationToken)
        {
            RepositoryModel repositoryModel;

            var userId = (await _mediator.Send(new GetUserQuery(request.Username))).Id;

            var currentEntity = await _service.FindAsync<RepositoryEntity>(
                x =>
                x.Name == request.Name &&
                x.UserId == userId &&
                x.Status == RecordStatuses.ACTIVE);

            if (currentEntity == null)
                throw new NotFoundException();

            currentEntity.IsPrivate = request.IsPrivate;
            currentEntity = _service.Update(currentEntity);
            await _service.SaveChangesAsync();

            repositoryModel = _mapper.Map<RepositoryEntity, RepositoryModel>(currentEntity);
            return repositoryModel;
        }
    }
}
