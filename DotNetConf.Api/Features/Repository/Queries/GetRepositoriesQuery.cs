using AutoMapper;
using DotNetConf.Api.Entities;
using DotNetConf.Api.Enums;
using DotNetConf.Api.Features.Repository.Models;
using DotNetConf.Api.Features.User.Queries;
using DotNetConf.Api.Infrastructures.Database;
using DotNetConf.Api.Models.Exceptions;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetConf.Api.Features.Repository.Queries
{
    public class GetRepositoriesQuery : IRequest<List<RepositoryModel>>
    {
        public GetRepositoriesQuery(string username)
        {
            this.Username = username;
        }
        public GetRepositoriesQuery(long userId)
        {
            this.UserId = userId;
        }
        public string Username { get; private set; }
        public long? UserId { get; private set; }
    }

    public class GetRepositoriesQueryHandler : IRequestHandler<GetRepositoriesQuery, List<RepositoryModel>>
    {
        private readonly IService _service;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetRepositoriesQueryHandler(IService service, IMapper mapper, IMediator mediator)
        {
            _service = service;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<List<RepositoryModel>> Handle(GetRepositoriesQuery request, CancellationToken cancellationToken)
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

            var repositoryEntities = await _service.GetListAsync<RepositoryEntity>(x => x.UserId == userId && x.Status == RecordStatuses.ACTIVE);

            return _mapper.Map<List<RepositoryEntity>, List<RepositoryModel>>(repositoryEntities);
        }
    }
}
