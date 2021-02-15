using AutoMapper;
using DotNetConf.Api.Controllers;
using DotNetConf.Api.Features.User.Commands;
using DotNetConf.Api.Features.User.Validators;
using DotNetConf.Api.Infrastructures.Database;
using DotNetConf.Api.Infrastructures.Mapper;
using DotNetConf.Api.Infrastructures.Middlewares;
using DotNetConf.Api.Infrastructures.Swaggers;
using DotNetConf.Api.Models;
using DotNetConf.Api.Models.Exceptions;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DotNetConf.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Authentication
            var appSettingsSection = Configuration.GetSection("Identity");
            services.Configure<IdentitySettingModel>(appSettingsSection);
            var identityModel = appSettingsSection.Get<IdentitySettingModel>();
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(identityModel.SecretKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = identityModel.Iss,
                ValidateAudience = true,
                ValidAudience = identityModel.Aud,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromDays(1),
                RequireExpirationTime = true,
            };

            services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.TokenValidationParameters = tokenValidationParameters;
            });
            #endregion

            services.AddDbContext<DotNetConfDbContext>(options => options.UseInMemoryDatabase(databaseName: "DotNetConfDb"));
            services.AddMediatR(typeof(CreateUserCommand).GetTypeInfo().Assembly);

            #region Services Register
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IService, Service>();
            #endregion

            #region AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region ApiVersioning & Swagger
            services.AddApiVersioning(
                options =>
                {
                    options.ReportApiVersions = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    //header version örneği
                    //options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                });
            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
            services.AddSwaggerGen(
                options =>
                {
                    options.OperationFilter<SwaggerDefaultValues>();
                    options.EnableAnnotations();
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Scheme = "Bearer",
                        Type = SecuritySchemeType.ApiKey,
                        BearerFormat = "JWT"
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    });
                });

            services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            #endregion

            #region CORS Enable
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .SetPreflightMaxAge(System.TimeSpan.FromMinutes(60))
                   .Build();
                });
            });
            #endregion

            #region Fluent Validation
            services.AddControllers()
                .AddFluentValidation(validationConfig => validationConfig.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(CreateUserValidator))))
                .AddNewtonsoftJson(setupAction => new Newtonsoft.Json.JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = c =>
                {
                    var messages = new List<string>();
                    foreach (var errors in c.ModelState.Values.Where(x => x.Errors.Count > 0).Select(x => x.Errors))
                    {
                        messages.AddRange(errors.Select(x => x.ErrorMessage).ToList());
                    }
                    throw new UnprocessableException(messages);
                };
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
