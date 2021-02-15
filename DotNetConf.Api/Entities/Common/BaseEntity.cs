using DotNetConf.Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetConf.Api.Entities.Common
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; private set; }
        public DateTime? UpdateDate { get; private set; }
        public RecordStatuses Status { get; private set; }

        public void Add()
        {
            CreateDate = DateTime.Now;
            UpdateDate = null;
            Status = RecordStatuses.ACTIVE;
        }

        public void Update()
        {
            UpdateDate = DateTime.Now;
            Status = RecordStatuses.ACTIVE;
        }

        public void Delete()
        {
            UpdateDate = DateTime.Now;
            Status = RecordStatuses.PASSIVE;
        }
    }
}
