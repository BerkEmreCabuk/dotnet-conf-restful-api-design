using AutoMapper;
using DotNetConf.Api.Entities;
using DotNetConf.Api.Enums;
using DotNetConf.Api.Features.Repository.Models;
using DotNetConf.Api.Features.Repository.Queries;
using DotNetConf.Api.Features.User.Queries;
using DotNetConf.Api.Infrastructures.Database;
using DotNetConf.Api.Models.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetConf.Api.Features.Repository.Commands
{
    public class CreateRepositoryCommand : RepositoryModel, IRequest<RepositoryModel>
    {
        public string Username { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateRepositoryCommand, RepositoryModel>
    {
        private readonly IService _service;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateUserCommandHandler(
            IService service,
            IMapper mapper,
            IMediator mediator)
        {
            _service = service;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<RepositoryModel> Handle(CreateRepositoryCommand request, CancellationToken cancellationToken)
        {
            var repositoryEntity = _mapper.Map<CreateRepositoryCommand, RepositoryEntity>(request);

            if (!string.IsNullOrEmpty(request.Username))
                repositoryEntity.UserId = (await _mediator.Send(new GetUserQuery(request.Username))).Id;
            //TODO else exist UserID

            var existName = await _mediator.Send(new ExistNameQuery(repositoryEntity.Name, repositoryEntity.UserId));            
            if (existName)
                throw new UnprocessableException("Repository Name bulunmaktadır.");
            
            repositoryEntity = _service.Add(repositoryEntity);
            await _service.SaveChangesAsync();

            return _mapper.Map<RepositoryEntity, RepositoryModel>(repositoryEntity);
        }
    }
}
