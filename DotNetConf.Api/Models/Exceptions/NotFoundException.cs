using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace DotNetConf.Api.Models.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException()
        {
            ResponseModel.Messages.Add("Not Found.");
            this.StatusCode = StatusCodes.Status404NotFound;
            this.Description = "Status for not found data or endpoint";
        }
        public NotFoundException(string message) : base(message)
        {
            this.StatusCode = StatusCodes.Status404NotFound;
        }

        public NotFoundException(List<string> messages)
        {
            ResponseModel.Messages = messages;
            this.StatusCode = StatusCodes.Status404NotFound;
        }

        public NotFoundException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            this.StatusCode = StatusCodes.Status404NotFound;
        }
    }
}
