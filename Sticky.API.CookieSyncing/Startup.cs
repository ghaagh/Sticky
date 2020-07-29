using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddRazorPages();
            services.Configure<CookieSyncingAPISetting>(Configuration.GetSection("Setting"));
            var client = new MongoClient("mongodb://localhost/");
            services.AddSingleton<IMongoClient, MongoClient>(c => client);
            services.AddSingleton<IPartnerCache, PartnerCache>();
            services.AddSingleton<ICrowlerCache, CrowlerCache>();
            services.AddSingleton<IRedisCache, RedisCache>();
            services.AddSingleton<IUserIdSetter, UserIdSetter>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
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
