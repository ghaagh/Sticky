using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Sticky.API.CookieSyncing.HttpMiddleware;
using Sticky.Models.Etc;
using Sticky.Repositories.Advertisement;
using Sticky.Repositories.Advertisement.Implementions;
using Sticky.Repositories.Common;
using Sticky.Repositories.Common.Implementions;

namespace Sticky.API.CookieSyncing
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<CookieSyncingAPISetting>(Configuration.GetSection("Setting"));
            var client = new MongoClient("mongodb://localhost/");
            services.AddSingleton<IMongoClient, MongoClient>(c => client);
            services.AddSingleton<IPartnerCache, PartnerCache>();
            services.AddSingleton<ICrowlerCache, CrowlerCache>();
            services.AddSingleton<IRedisCache, RedisCache>();
            services.AddSingleton<IUserIdSetter, UserIdSetter>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.MapWhen(c => c.Request.Path.ToString().EndsWith("sync.html"), b => { b.UserCookieSyncing(); });
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UserCookieSyncing(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CookieSyncHandler>();
        }
    }
}
