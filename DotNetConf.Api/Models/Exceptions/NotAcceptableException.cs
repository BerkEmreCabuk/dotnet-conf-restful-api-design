using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace DotNetConf.Api.Models.Exceptions
{
    public class NotAcceptableException : BaseException
    {
        public NotAcceptableException()
        {
            ResponseModel.Messages.Add("Request Invalid Format");
            this.StatusCode = StatusCodes.Status406NotAcceptable;
            this.Description = "Status for request invalid format";
        }
        public NotAcceptableException(string message) : base(message)
        {
            this.StatusCode = StatusCodes.Status406NotAcceptable;
        }

        public NotAcceptableException(List<string> messages)
        {
            ResponseModel.Messages = messages;
            this.StatusCode = StatusCodes.Status406NotAcceptable;
        }

        public NotAcceptableException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            this.StatusCode = StatusCodes.Status406NotAcceptable;
        }
    }
}
