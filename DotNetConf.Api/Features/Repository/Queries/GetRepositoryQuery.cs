using AutoMapper;
using DotNetConf.Api.Entities;
using DotNetConf.Api.Enums;
using DotNetConf.Api.Features.Repository.Models;
using DotNetConf.Api.Features.User.Queries;
using DotNetConf.Api.Infrastructures.Database;
using DotNetConf.Api.Models.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetConf.Api.Features.Repository.Queries
{
    public class GetRepositoryQuery : IRequest<RepositoryModel>
    {
        public GetRepositoryQuery(string username, string name)
        {
            this.Username = username;
            this.Name = name;
        }
        public GetRepositoryQuery(long userId, long id)
        {
            this.UserId = userId;
            this.Id = id;
        }
        public string Username { get; private set; }
        public string Name { get; private set; }
        public long? UserId { get; private set; }
        public long? Id { get; private set; }
    }

    public class GetRepositoryQueryHandler : IRequestHandler<GetRepositoryQuery, RepositoryModel>
    {
        private readonly IService _service;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetRepositoryQueryHandler(IService service, IMapper mapper, IMediator mediator)
        {
            _service = service;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<RepositoryModel> Handle(GetRepositoryQuery request, CancellationToken cancellationToken)
        {
            long userId;
            if (!string.IsNullOrEmpty(request.Username))
            {
                userId = (await _mediator.Send(new GetUserQuery(request.Username))).Id;
            }
            else if (request.UserId.HasValue)
            {
                userId = request.UserId.Value;
            }
            else
            {
                throw new UnprocessableException();
            }

            var repositoryEntity = await _service.FindAsync<RepositoryEntity>(x =>
            x.UserId == userId &&
            x.Status == RecordStatuses.ACTIVE &&
            (!string.IsNullOrEmpty(request.Name) && x.Name == request.Name) || (request.Id.HasValue && x.Id == request.Id.Value),
            x => x.User);

            return _mapper.Map<RepositoryEntity, RepositoryModel>(repositoryEntity);
        }
    }
}
