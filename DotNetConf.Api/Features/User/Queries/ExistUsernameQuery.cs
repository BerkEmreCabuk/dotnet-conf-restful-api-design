using AutoMapper;
using DotNetConf.Api.Entities;
using DotNetConf.Api.Enums;
using DotNetConf.Api.Features.User.Models;
using DotNetConf.Api.Infrastructures.Database;
using DotNetConf.Api.Models.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetConf.Api.Features.User.Queries
{
    public class ExistUsernameQuery : IRequest<bool>
    {
        public ExistUsernameQuery(string Username)
        {
            this.Username = Username;
        }
        public string Username { get; set; }
    }

    public class ExistUsernameQueryHandler : IRequestHandler<ExistUsernameQuery, bool>
    {
        private readonly IService _service;
        private readonly IMapper _mapper;

        public ExistUsernameQueryHandler(IService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<bool> Handle(ExistUsernameQuery request, CancellationToken cancellationToken)
        {
            return await _service.ExistAsync<UserEntity>(x => x.Username == request.Username && x.Status == RecordStatuses.ACTIVE);
        }
    }
}
