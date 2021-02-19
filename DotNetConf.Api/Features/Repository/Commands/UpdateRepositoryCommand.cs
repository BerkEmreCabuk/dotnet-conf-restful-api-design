using AutoMapper;
using DotNetConf.Api.Entities;
using DotNetConf.Api.Enums;
using DotNetConf.Api.Features.Repository.Models;
using DotNetConf.Api.Features.User.Queries;
using DotNetConf.Api.Infrastructures.Database;
using DotNetConf.Api.Infrastructures.Mapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetConf.Api.Features.Repository.Commands
{
    public class UpdateRepositoryCommand : IRequest<(RepositoryModel, bool)>, IMapping
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsFork { get; set; }

        public void CreateMappings(IProfileExpression profileExpression)
        {
            profileExpression.CreateMap<UpdateRepositoryCommand, CreateRepositoryCommand>();
            profileExpression.CreateMap<UpdateRepositoryCommand, RepositoryEntity>();
            profileExpression.CreateMap<RepositoryEntity, UpdateRepositoryCommand>();
        }
    }

    public class UpdateRepositoryCommandHandler : IRequestHandler<UpdateRepositoryCommand, (RepositoryModel, bool)>
    {
        private readonly IService _service;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UpdateRepositoryCommandHandler(
            IService service,
            IMapper mapper,
            IMediator mediator)
        {
            _service = service;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<(RepositoryModel, bool)> Handle(UpdateRepositoryCommand request, CancellationToken cancellationToken)
        {
            RepositoryModel repositoryModel;
            bool isCreated = false;

            if (!string.IsNullOrEmpty(request.Username))
                request.UserId = (await _mediator.Send(new GetUserQuery(request.Username))).Id;

            var currentEntity = await _service.FindAsync<RepositoryEntity>(
                x =>
                x.Name == request.Name &&
                x.UserId == request.UserId &&
                x.Status == RecordStatuses.ACTIVE, true);

            if (currentEntity == null)
            {
                repositoryModel = await _mediator.Send(_mapper.Map<UpdateRepositoryCommand, CreateRepositoryCommand>(request));
                isCreated = true;
            }
            else
            {
                currentEntity = _mapper.Map(request, currentEntity);
                currentEntity = _service.Update(currentEntity);
                await _service.SaveChangesAsync();
                repositoryModel = _mapper.Map<RepositoryEntity, RepositoryModel>(currentEntity);
            }

            return (repositoryModel, isCreated);
        }
    }
}
