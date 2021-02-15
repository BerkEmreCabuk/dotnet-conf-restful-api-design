using DotNetConf.Api.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetConf.Api.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Company { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public ICollection<RepositoryEntity> Repositories { get; set; }
    }
}
