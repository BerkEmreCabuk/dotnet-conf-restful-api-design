using DotNetConf.Api.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetConf.Api.Entities
{
    public class RepositoryEntity : BaseEntity
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsFork { get; set; }
        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
    }
}
