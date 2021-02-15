using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetConf.Api.Enums
{
    public enum RecordStatuses:byte
    {
        UNKNOWN = 0,
        ACTIVE = 1,
        PASSIVE = 2
    }
}
