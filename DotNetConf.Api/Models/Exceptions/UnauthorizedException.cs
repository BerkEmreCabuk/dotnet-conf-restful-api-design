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
            ProblemDetailsModel.Title = "Token expire or unauthantication";
            ProblemDetailsModel.Detail = "Status for token expire or unauthantication";
            ProblemDetailsModel.Status = StatusCodes.Status401Unauthorized;
        }

        public UnauthorizedException(string message) : base(message)
        {
            ProblemDetailsModel.Status = StatusCodes.Status401Unauthorized;
        }

        public UnauthorizedException(List<string> messages) : base(messages)
        {
            ProblemDetailsModel.Status = StatusCodes.Status401Unauthorized;
        }

        public UnauthorizedException(string title, List<string> messages) : base(title, messages)
        {
            ProblemDetailsModel.Status = StatusCodes.Status401Unauthorized;
        }

        public UnauthorizedException(string message, string title) : base(message, title)
        {
            ProblemDetailsModel.Status = StatusCodes.Status401Unauthorized;
        }

        public UnauthorizedException(string message, string title, IDictionary<string, object> extensions) : base(message, title, extensions)
        {
            ProblemDetailsModel.Status = StatusCodes.Status401Unauthorized;
        }
    }
}
