using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sticky.API.Script.HttpMiddleware;
using Sticky.Models.Context;
using Sticky.Models.Etc;
using Sticky.Repositories.Advertisement;
using Sticky.Repositories.Advertisement.Implementions;
using Sticky.Repositories.Common;
using Sticky.Repositories.Common.Implementions;
using Sticky.Repositories.Script;
using Sticky.Repositories.Script.Implementions;

namespace Sticky.API.Script
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
            services.AddCors();
            services.AddMvc();
            var appSetting = Configuration.GetSection("Setting");
            services.Configure<ScriptAPISetting>(appSetting);
            var connectionString = appSetting.Get<ScriptAPISetting>().ConnectionString;
            services.AddDbContext<StickyDbContext>(options => options.UseSqlServer(connectionString));
            services
            .AddSingleton<ITotalVisitUpdater, TotalVisitUpdater>()
            .AddSingleton<IUtility, Utility>()
            .AddSingleton<ICrowlerCache, CrowlerCache>()
            .AddSingleton<IHostCache, HostCache>()
            .AddSingleton<IKafkaClient, KafkaClient>()
            .AddSingleton<IKafkaLogProducer, KafkaLogProducer>()
            .AddSingleton<ICategoryLogger, CategoryLogger>()
            .AddSingleton<IErrorLogger, ErrorLogger>()
            .AddSingleton<IRedisCache, RedisCache>()
            .AddSingleton<IPartnerCache, PartnerCache>()
            .AddSingleton<IProductCache, ProductCache>()
            .AddSingleton<IHostScriptChecker, HostScriptChecker>()
            .AddSingleton<IUserIdSetter, UserIdSetter>()
            .AddSingleton<ICookieSyncCache, CookieSyncCache>();
        }
        public static bool SetOriginAllowed(string host)
        {
            RedisCache rediscache = new RedisCache();
            return new HostCache(rediscache).HostExists(host);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            IServiceScopeFactory serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            app.UseDeveloperExceptionPage();
            app.UseCors(builder => builder.SetIsOriginAllowed(SetOriginAllowed).AllowAnyHeader().AllowAnyMethod());

            app.MapWhen(context => context.Request.Path.ToString().EndsWith("iframe.html"),
                appBuilder =>
                {
                    appBuilder.UseIframeMiddlewareExtension();
                });
            app.UseStaticFiles();
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
}
