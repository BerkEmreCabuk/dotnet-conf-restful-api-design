using AutoMapper;
using DotNetConf.Api.Entities;
using DotNetConf.Api.Enums;
using DotNetConf.Api.Features.User.Models;
using DotNetConf.Api.Infrastructures.Database;
using DotNetConf.Api.Models.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetConf.Api.Features.User.Commands
{
    public class CreateUserCommand : UserModel, IRequest<UserModel>
    {
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserModel>
    {
        private readonly IService _service;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(
            IService service,
            IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<UserModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existUsername = await _service.ExistAsync<UserEntity>(x =>
              x.Username == request.Username &&
              x.Status == RecordStatuses.ACTIVE);

            if (existUsername)
                throw new UnprocessableException("Username bulunmaktadır.");

            var userEntity = _mapper.Map<UserEntity>(request);
            userEntity = _service.Add(userEntity);
            await _service.SaveChangesAsync();

            return _mapper.Map<UserEntity, UserModel>(userEntity);
        }
    }
}
