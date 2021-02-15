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
            ResponseModel.Messages.Add("Request Model Invalid or Missing");
            this.StatusCode = StatusCodes.Status422UnprocessableEntity;
            this.Description = "Status for request model invalid or missing";
        }
        public UnprocessableException(string message) : base(message)
        {
            this.StatusCode = StatusCodes.Status422UnprocessableEntity;
        }

        public UnprocessableException(List<string> messages)
        {
            ResponseModel.Messages = messages;
            this.StatusCode = StatusCodes.Status422UnprocessableEntity;
        }

        public UnprocessableException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            this.StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
