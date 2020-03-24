using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sticky.Models.Context;
using Sticky.Models.Etc;
using Sticky.Repositories.Advertisement;
using Sticky.Repositories.Advertisement.Implementions;
using Sticky.Repositories.Common;
using Sticky.Repositories.Common.Implementions;

namespace Sticky.API.Advertisement
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

            var appSetting = Configuration.GetSection("Setting");
            services.Configure<AdvertisementAPISetting>(appSetting);
            var connectionString = appSetting.Get<DashboardAPISetting>().ConnectionString;
            services.AddDbContext<StickyDbContext>(options => options.UseSqlServer(connectionString));
            //services.AddSingleton<ISegmentUpdater, SegmentUpdater>();

            services.AddSingleton<ITotalVisitUpdater, TotalVisitUpdater>();
            services.AddSingleton<IPartnerCache, PartnerCache>();
            services.AddSingleton<IResultCache, ResultCache>();
            services.AddSingleton<IAwesomeTextGenerator, AwesomeTextGenerator>();
            services.AddSingleton<IResponseTimeLogger, ResponseTimeLogger>();
            services.AddSingleton<ISegmentCache, SegmentCache>();
            services.AddSingleton<IClickLogger, ClickLogger>();
            services.AddSingleton<IUserMembershipFinder, UserMembershipFinder>();
            services.AddSingleton<IEncodeDecodeManager, EncodeDecodeManager>();
            services.AddSingleton<IHostCache, HostCache>();
            services.AddSingleton<IRedisCache, RedisCache>();
            services.AddSingleton<IProductCache, ProductCache>();
            services.AddSingleton<IResponseGenerator, ResponseGenerator>();
            services.AddSingleton<ICookieSyncCache, CookieSyncCache>();
            var client = new MongoClient("mongodb://localhost/");
            services.AddSingleton<IMongoClient, MongoClient>(c => client);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
