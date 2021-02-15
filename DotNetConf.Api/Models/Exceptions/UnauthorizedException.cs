using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetConf.Api.Models.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException()
        {
            ResponseModel.Messages.Add("token expire or unauthantication");
            this.StatusCode = StatusCodes.Status401Unauthorized;
            this.Description = "Status for token expire or unauthantication";
        }
        public UnauthorizedException(string message) : base(message)
        {
            this.StatusCode = StatusCodes.Status401Unauthorized;
        }

        public UnauthorizedException(List<string> messages)
        {
            ResponseModel.Messages = messages;
            this.StatusCode = StatusCodes.Status401Unauthorized;
        }

        public UnauthorizedException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            this.StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
