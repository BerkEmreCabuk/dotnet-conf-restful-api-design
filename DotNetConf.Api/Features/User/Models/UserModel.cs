using AutoMapper;
using DotNetConf.Api.Entities;
using DotNetConf.Api.Infrastructures.Mapper;

namespace DotNetConf.Api.Features.User.Models
{
    public class UserModel : IMapping
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Company { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }

        public void CreateMappings(IProfileExpression profileExpression)
        {
            profileExpression.CreateMap<UserModel, UserEntity>();
            profileExpression.CreateMap<UserEntity, UserModel>();
        }
    }
}
