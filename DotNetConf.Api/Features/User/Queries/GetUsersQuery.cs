using AutoMapper;
using DotNetConf.Api.Entities;
using DotNetConf.Api.Features.User.Models;
using DotNetConf.Api.Infrastructures.Database;
using DotNetConf.Api.Models.Exceptions;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetConf.Api.Features.User.Queries
{
    public class GetUsersQuery : IRequest<List<UserModel>>
    {
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserModel>>
    {
        private readonly IService _service;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<List<UserModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var entity = await _service.GetListAsync<UserEntity>();
            return _mapper.Map<List<UserEntity>,List<UserModel>>(entity);
        }
    }
}
