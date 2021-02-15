using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetConf.Api.Models
{
    public class IdentitySettingModel
    {
        public string SecretKey { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
    }
}
