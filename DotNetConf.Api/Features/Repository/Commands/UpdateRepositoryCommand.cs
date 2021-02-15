using AutoMapper;
using DotNetConf.Api.Entities;
using DotNetConf.Api.Enums;
using DotNetConf.Api.Features.Repository.Models;
using DotNetConf.Api.Features.User.Models;
using DotNetConf.Api.Features.User.Queries;
using DotNetConf.Api.Infrastructures.Database;
using DotNetConf.Api.Infrastructures.Mapper;
using DotNetConf.Api.Models.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetConf.Api.Features.Repository.Commands
{
    public class UpdateRepositoryCommand : RepositoryModel, IRequest<(RepositoryModel, bool)>, IMapping
    {
        public string Username { get; set; }
        public void CreateMappings(IProfileExpression profileExpression)
        {
            profileExpression.CreateMap<UpdateRepositoryCommand, CreateRepositoryCommand>();
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
                currentEntity = _mapper.Map<RepositoryModel, RepositoryEntity>(request, currentEntity);
                currentEntity = _service.Update(currentEntity);
                await _service.SaveChangesAsync();
                repositoryModel = _mapper.Map<RepositoryEntity, RepositoryModel>(currentEntity);
            }

            return (repositoryModel, isCreated);
        }
    }
}
