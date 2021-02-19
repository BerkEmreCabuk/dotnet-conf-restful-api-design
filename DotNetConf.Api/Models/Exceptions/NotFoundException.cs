using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace DotNetConf.Api.Models.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException()
        {
            ProblemDetailsModel.Title = "Not Found";
            ProblemDetailsModel.Detail = "Status for not found data or endpoint";
            ProblemDetailsModel.Status = StatusCodes.Status404NotFound;
        }

        public NotFoundException(string message) : base(message)
        {
            ProblemDetailsModel.Status = StatusCodes.Status404NotFound;
        }

        public NotFoundException(List<string> messages) : base(messages)
        {
            ProblemDetailsModel.Status = StatusCodes.Status404NotFound;
        }

        public NotFoundException(string title, List<string> messages) : base(title, messages)
        {
            ProblemDetailsModel.Status = StatusCodes.Status404NotFound;
        }

        public NotFoundException(string message, string title) : base(message, title)
        {
            ProblemDetailsModel.Status = StatusCodes.Status404NotFound;
        }

        public NotFoundException(string message, string title, IDictionary<string, object> extensions) : base(message, title, extensions)
        {
            ProblemDetailsModel.Status = StatusCodes.Status404NotFound;
        }
    }
}
