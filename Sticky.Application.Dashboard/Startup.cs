using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sticky.Application.Dashboard.Extensions;
using Sticky.Application.Dashboard.Extensions.Extensions;
using Sticky.Application.Dashboard.Services;
using Sticky.Infrastructure.Sql;
using Sticky.Infrastructure.Swagger;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Sticky.Application.Dashboard
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

            services.AddSql(Configuration.GetConnectionString("Sticky"));

            services.AddScoped<ITokenGenerator, TokenGenerator>();

            services.AddSwagger(Configuration.GetSection("Swagger").Get<SwaggerConfig>());

            services.AddJWT(settings => Configuration.Bind("JWT", settings));

            services.AddMediatR(Assembly.GetExecutingAssembly());
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            app.UseSwagger();
            var swaggerConfig = Configuration.GetSection("Swagger").Get<SwaggerConfig>();
            app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{swaggerConfig.Version}/swagger.json", swaggerConfig.Title));
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.ConfigureExceptionMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
