using DotNetConf.Api.Models.BaseModels;
using DotNetConf.Api.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotNetConf.Api.Infrastructures.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(
            RequestDelegate next
        )
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (!string.IsNullOrEmpty(context?.Request?.ContentType) && context.Request.ContentType != "application/json")
                    throw new NotAcceptableException();
                await _next.Invoke(context);
            }
            catch (Exception exception)
            {
                context.Response.ContentType = "application/problem+json";
                var baseException = exception is BaseException ? (BaseException)exception : new InternalServerError();
                baseException.ProblemDetailsModel.Instance = context.Request.Path;
                baseException.ProblemDetailsModel.Type = context.Request.Path;
                context.Response.StatusCode = baseException.ProblemDetailsModel.Status.Value;
                await context.Response.WriteAsync(JsonSerializer.Serialize(baseException.ProblemDetailsModel));
            }
        }
    }
}
