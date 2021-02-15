using DotNetConf.Api.Models.BaseModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace DotNetConf.Api.Models.Exceptions
{
    public class BaseException : Exception
    {
        public BaseResponseModel ResponseModel = new BaseResponseModel();
        public int StatusCode { get; set; }
        public string Description { get; set; }
        public BaseException() : base()
        {
            this.StatusCode = StatusCodes.Status500InternalServerError;
        }
        public BaseException(string message) : base(message)
        {
            ResponseModel.Messages.Add(message);
            this.StatusCode = StatusCodes.Status500InternalServerError;
        }

        public BaseException(List<string> messages)
        {
            ResponseModel.Messages.AddRange(messages);
            this.StatusCode = StatusCodes.Status500InternalServerError;
        }

        public BaseException(string message, Exception innerException, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            ResponseModel.Messages.Add(message);
            this.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
