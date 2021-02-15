using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetConf.Api.Models.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException()
        {
            ResponseModel.Messages.Add("Error Occured");
            this.StatusCode = StatusCodes.Status400BadRequest;
            this.Description = "Status for error occurred";
        }
        public BadRequestException(string message) : base(message)
        {
            this.StatusCode = StatusCodes.Status400BadRequest;
        }

        public BadRequestException(List<string> messages)
        {
            ResponseModel.Messages = messages;
            this.StatusCode = StatusCodes.Status400BadRequest;
        }

        public BadRequestException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            this.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
