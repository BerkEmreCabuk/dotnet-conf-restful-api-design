using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace DotNetConf.Api.Models.Exceptions
{
    public class NotAcceptableException : BaseException
    {
        public NotAcceptableException()
        {
            ProblemDetailsModel.Title = "Request Invalid Format";
            ProblemDetailsModel.Detail = "Status for request invalid format";
            ProblemDetailsModel.Status = StatusCodes.Status406NotAcceptable;
        }

        public NotAcceptableException(string message) : base(message)
        {
            ProblemDetailsModel.Status = StatusCodes.Status406NotAcceptable;
        }

        public NotAcceptableException(List<string> messages) : base(messages)
        {
            ProblemDetailsModel.Status = StatusCodes.Status406NotAcceptable;
        }

        public NotAcceptableException(string title, List<string> messages) : base(title, messages)
        {
            ProblemDetailsModel.Status = StatusCodes.Status406NotAcceptable;
        }

        public NotAcceptableException(string message, string title) : base(message, title)
        {
            ProblemDetailsModel.Status = StatusCodes.Status406NotAcceptable;
        }

        public NotAcceptableException(string message, string title, IDictionary<string, object> extensions) : base(message, title, extensions)
        {
            ProblemDetailsModel.Status = StatusCodes.Status406NotAcceptable;
        }
    }
}
