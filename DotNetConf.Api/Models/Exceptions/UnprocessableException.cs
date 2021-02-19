using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetConf.Api.Models.Exceptions
{
    public class UnprocessableException : BaseException
    {
        public UnprocessableException()
        {
            ProblemDetailsModel.Title = "Request Model Invalid or Missing";
            ProblemDetailsModel.Detail = "Status for request model invalid or missing";
            ProblemDetailsModel.Status = StatusCodes.Status422UnprocessableEntity;
        }

        public UnprocessableException(string message) : base(message)
        {
            ProblemDetailsModel.Status = StatusCodes.Status422UnprocessableEntity;
        }

        public UnprocessableException(List<string> messages) : base(messages)
        {
            ProblemDetailsModel.Status = StatusCodes.Status422UnprocessableEntity;
        }

        public UnprocessableException(string title, List<string> messages) : base(title, messages)
        {
            ProblemDetailsModel.Status = StatusCodes.Status422UnprocessableEntity;
        }

        public UnprocessableException(string message, string title) : base(message, title)
        {
            ProblemDetailsModel.Status = StatusCodes.Status422UnprocessableEntity;
        }

        public UnprocessableException(string message, string title, IDictionary<string, object> extensions) : base(message, title, extensions)
        {
            ProblemDetailsModel.Status = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
