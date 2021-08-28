using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Sticky.Application.Advertising.Extensions;
using Sticky.Application.Advertising.Services;
using Sticky.Infrastructure.Mongo;
using Sticky.Infrastructure.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Application.Advertising
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Setting>(Configuration.GetSection("Setting"))
                
                .AddRedis(Configuration.GetConnectionString("Redis"))
                
                .AddRedisCache()
                
                .AddRequestResponseRepository()
                
                .AddMongo(Configuration.GetSection("Mongo").Get<MongoConfig>())
                
                .AddSwagger(Configuration.GetSection("Swagger").Get<SwaggerConfig>())
                
                .AddSingleton<IEncodeDecodeManager, EncodeDecodeManager>()
                
                .AddSingleton<IMembershipFinder, MembershipFinder>()
                
                .AddSingleton<IAdTextGenerator, AddTextGenerator>();
           
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {      
            app.UseSwagger();
            var swaggerConfig = Configuration.GetSection("Swagger").Get<SwaggerConfig>();
            app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{swaggerConfig.Version}/swagger.json", swaggerConfig.Title));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
