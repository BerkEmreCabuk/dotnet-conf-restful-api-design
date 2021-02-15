using AutoMapper;
using DotNetConf.Api.Entities;
using DotNetConf.Api.Enums;
using DotNetConf.Api.Infrastructures.Database;
using DotNetConf.Api.Features.User.Queries;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetConf.Api.Features.Repository.Queries
{
    public class ExistNameQuery : IRequest<bool>
    {
        public ExistNameQuery(string name, string userName)
        {
            this.Name = Name;
            this.UserName = userName;
        }
        public ExistNameQuery(string name, long userId)
        {
            this.Name = Name;
            this.UserId = userId;
        }
        public string Name { get; private set; }
        public long UserId { get; private set; }
        public string UserName { get; private set; }
    }

    public class ExistNameQueryHandler : IRequestHandler<ExistNameQuery, bool>
    {
        private readonly IService _service;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ExistNameQueryHandler(IService service, IMapper mapper, IMediator mediator)
        {
            _service = service;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<bool> Handle(ExistNameQuery request, CancellationToken cancellationToken)
        {
            long userId;
            if(!string.IsNullOrEmpty(request.UserName))
            {
                userId = (await _mediator.Send(new GetUserQuery(request.UserName))).Id;
            }
            else
            {
                userId = request.UserId;
            }

            return await _service.ExistAsync<RepositoryEntity>(
                x =>
                x.Name == request.Name &&
                x.UserId == userId &&
                x.Status == RecordStatuses.ACTIVE);
        }
    }
}
