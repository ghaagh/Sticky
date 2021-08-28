using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace Sticky.Infrastructure.Swagger
{
    public static class AddMongoExtension
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, SwaggerConfig config)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(config.Version, new OpenApiInfo { Title = config.Title, Version = config.Version });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    BearerFormat="JWT",
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Authorization:Bearer <your-bearer-token>"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
            });
            return services;
        }
    }
}
