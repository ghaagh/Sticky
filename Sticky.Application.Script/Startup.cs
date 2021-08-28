using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sticky.Application.Script.Extensions;
using Sticky.Infrastructure.Swagger;
using Sticky.Application.Script.Services;
using Sticky.Domain.ClientUsers;
using Sticky.Infrastructure.Redis;
using Sticky.Application.Script.Extensions.Extensions;
using System.Text.Json.Serialization;
using Sticky.Infrastructure.Message.Kafka.Extensions;

namespace Sticky.Application.Script
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

            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.Configure<Setting>(Configuration.GetSection("Setting"));

            services.AddKafka(settings => Configuration.Bind("Kafka", settings));

            services.AddSingleton<IUtility, Utility>()
                .AddSingleton<IClientUserRepository, RedisClientUserRepository>();
            
            services.UseRedisCache(Configuration.GetConnectionString("Redis"));

            services.AddSwagger(Configuration.GetSection("Swagger").Get<SwaggerConfig>());
        }

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

            app.MapWhen(context => context.Request.Path.ToString().EndsWith("iframe.html"),
                appBuilder =>
                {
                appBuilder.UseCustomHanlderMiddleware();
                });
            app.ConfigureExceptionMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
