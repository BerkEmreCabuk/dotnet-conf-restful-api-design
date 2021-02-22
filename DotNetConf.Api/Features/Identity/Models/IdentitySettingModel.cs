namespace DotNetConf.Api.Features.Identity.Models
{
    public class IdentitySettingModel
    {
        public string SecretKey { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
    }
}
