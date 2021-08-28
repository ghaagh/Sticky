using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sticky.Application.CookieSyncing.Extensions;
using Sticky.Domain.ClientUsers;
using Sticky.Infrastructure.Mongo;
using Sticky.Infrastructure.Redis;

namespace Sticky.Application.CookieSyncing
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

            services.AddControllers();

            services.UseRedisCache(Configuration.GetConnectionString("Redis"));
            services.AddSingleton<IClientUserRepository, RedisClientUserRepository>();
            services.AddMongo(Configuration.GetSection("Mongo").Get<MongoConfig>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapWhen(c => c.Request.Path.ToString().EndsWith("sync.html"), b => { b.UseCookieSyncMiddleware(); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
