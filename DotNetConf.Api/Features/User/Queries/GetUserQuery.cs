using AutoMapper;
using DotNetConf.Api.Entities;
using DotNetConf.Api.Enums;
using DotNetConf.Api.Features.User.Models;
using DotNetConf.Api.Infrastructures.Database;
using DotNetConf.Api.Models.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetConf.Api.Features.User.Queries
{
    public class GetUserQuery : IRequest<UserModel>
    {
        public GetUserQuery(string username)
        {
            this.UserName = username;
        }

        public GetUserQuery(long id)
        {
            this.Id = id;
        }
        public string UserName { get; private set; }
        public long? Id { get; private set; }
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserModel>
    {
        private readonly IService _service;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<UserModel> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var entity = await _service
            .FindAsync<UserEntity>(x =>
                x.Status == RecordStatuses.ACTIVE &&
                ((!string.IsNullOrEmpty(request.UserName) && x.Username == request.UserName) || (request.Id.HasValue && x.Id == request.Id)));

            if (entity == null)
                throw new NotFoundException();

            return _mapper.Map<UserModel>(entity);
        }
    }
}
