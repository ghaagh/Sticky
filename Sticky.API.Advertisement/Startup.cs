﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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
            services.AddSingleton<IUtility, Utility>();
            services.AddSingleton<IHostCache, HostCache>();
            services.AddSingleton<IRedisCache, RedisCache>();
            services.AddSingleton<IProductCache, ProductCache>();
            services.AddSingleton<IResponseGenerator, ResponseGenerator>();
            services.AddSingleton<ICookieSyncCache, CookieSyncCache>();
            var client = new MongoClient("mongodb://localhost/");
            services.AddSingleton<IMongoClient, MongoClient>(c => client);
            services.AddSingleton<IApiDescriptionGroupCollectionProvider, ApiDescriptionGroupCollectionProvider>();
            services.AddTransient<IApiDescriptionProvider, DefaultApiDescriptionProvider>();
            services.AddRazorPages();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Advertisement API",
                    Description = "For returning ad type images and text",
                    TermsOfService = new Uri("https://example.com/terms"),
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sticky Advertisement API V1");
            });
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
