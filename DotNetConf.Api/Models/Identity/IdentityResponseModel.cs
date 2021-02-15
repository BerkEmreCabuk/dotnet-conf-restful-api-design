using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetConf.Api.Models.Identity
{
    public class IdentityResponseModel
    {
        public IdentityResponseModel(string token)
        {
            this.Token = token;
            this.ExpiresDate = DateTime.Now.AddDays(1);
        }
        public string Token { get; set; }
        public DateTime ExpiresDate { get; set; }
    }
}
