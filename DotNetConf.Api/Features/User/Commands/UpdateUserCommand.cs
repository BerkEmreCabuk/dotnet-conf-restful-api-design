using AutoMapper;
using DotNetConf.Api.Entities;
using DotNetConf.Api.Enums;
using DotNetConf.Api.Features.User.Models;
using DotNetConf.Api.Infrastructures.Database;
using DotNetConf.Api.Infrastructures.Mapper;
using DotNetConf.Api.Models.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetConf.Api.Features.User.Commands
{
    public class UpdateUserCommand : UserModel, IRequest<(UserModel, bool)>, IMapping
    {
        public void CreateMappings(IProfileExpression profileExpression)
        {
            profileExpression.CreateMap<UpdateUserCommand, CreateUserCommand>();
        }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, (UserModel, bool)>
    {
        private readonly IService _service;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UpdateUserCommandHandler(
            IService service,
            IMapper mapper,
            IMediator mediator)
        {
            _service = service;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<(UserModel, bool)> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            UserModel userModel;
            bool isCreated = false;
            var currentEntity = await _service.Query<UserEntity>().FirstOrDefaultAsync(x => x.Username == request.Username && x.Status == RecordStatuses.ACTIVE);
            if (currentEntity == null)
            {
                userModel = await _mediator.Send(_mapper.Map<UpdateUserCommand, CreateUserCommand>(request));
                isCreated = true;
            }
            else
            {
                currentEntity = _mapper.Map<UserModel, UserEntity>(request, currentEntity);
                currentEntity = _service.Update(currentEntity);
                await _service.SaveChangesAsync();
                userModel = _mapper.Map<UserEntity, UserModel>(currentEntity);
            }

            return (userModel, isCreated);
        }
    }
}
