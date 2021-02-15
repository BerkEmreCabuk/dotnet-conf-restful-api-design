using DotNetConf.Api.Models.Exceptions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace DotNetConf.Api.Infrastructures.Swaggers
{
    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated |= apiDescription.IsDeprecated();

            if (operation.Parameters == null)
            {
                return;
            }

            foreach (var response in operation.Responses)
            {
                BaseException baseException;
                switch (response.Key)
                {
                    case "400":
                        baseException = new BadRequestException();
                        break;
                    case "401":
                        baseException = new UnauthorizedException();
                        break;
                    case "403":
                        baseException = new UnauthorizedException();
                        break;
                    case "404":
                        baseException = new NotFoundException();
                        break;
                    case "406":
                        baseException = new NotAcceptableException();
                        break;
                    case "422":
                        baseException = new UnprocessableException();
                        break;
                    case "500":
                        baseException = new InternalServerError();
                        break;
                    default:
                        baseException = null;
                        break;
                }
                if (baseException != null)
                {
                    response.Value.Description = baseException.Description;
                }
            }

            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                if (String.IsNullOrEmpty(parameter.Description) && !String.IsNullOrEmpty(description?.ModelMetadata?.Description))
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (parameter.Schema.Default == null && description.DefaultValue != null)
                {
                    parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
                }

                parameter.Required |= description.IsRequired;
            }
        }
    }
}
