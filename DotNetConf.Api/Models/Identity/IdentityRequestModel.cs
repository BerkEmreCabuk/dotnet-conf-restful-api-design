﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetConf.Api.Models.Identity
{
    public class IdentityRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
