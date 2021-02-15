using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetConf.Api.Models.Exceptions
{
    public class InternalServerError : BaseException
    {
        public InternalServerError()
        {
            ResponseModel.Messages.Add("Unexpected Error Occured");
            this.StatusCode = StatusCodes.Status500InternalServerError;
            this.Description = "Status for internal server error";
        }
        public InternalServerError(string message) : base(message)
        {
            this.StatusCode = StatusCodes.Status500InternalServerError;
        }

        public InternalServerError(List<string> messages)
        {
            ResponseModel.Messages = messages;
            this.StatusCode = StatusCodes.Status500InternalServerError;
        }

        public InternalServerError(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            this.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
