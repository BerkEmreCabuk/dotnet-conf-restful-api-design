using AutoMapper;
using DotNetConf.Api.Entities;
using DotNetConf.Api.Features.User.Models;
using DotNetConf.Api.Infrastructures.Mapper;
using System.Collections.Generic;

namespace DotNetConf.Api.Features.Repository.Models
{
    public class RepositoryModel : IMapping
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsFork { get; set; }
        public UserModel User { get; set; }

        public void CreateMappings(IProfileExpression profileExpression)
        {
            profileExpression.CreateMap<RepositoryModel, RepositoryEntity>();
            profileExpression.CreateMap<RepositoryEntity, RepositoryModel>();
        }
    }
}
