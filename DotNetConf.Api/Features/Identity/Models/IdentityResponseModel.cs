using System;

namespace DotNetConf.Api.Features.Identity.Models
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
