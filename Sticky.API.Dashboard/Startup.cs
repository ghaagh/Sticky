using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sticky.Models.Context;
using Sticky.Models.Etc;
using Sticky.Repositories.Dashboard;
using Sticky.Repositories.Dashboard.Implementions;
using Sticky.Repositories.Common;
using Sticky.Repositories.Common.Implementions;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;

namespace Sticky.API.Dashboard
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

            var appSetting = Configuration.GetSection("Setting");
            services.Configure<DashboardAPISetting>(appSetting);
            var key = Encoding.ASCII.GetBytes(appSetting.Get<DashboardAPISetting>().JWTSecret);
            var connectionString = appSetting.Get<DashboardAPISetting>().ConnectionString;
            services.AddDbContext<StickyDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Sticky.API.Dashboard")));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<StickyDbContext>().AddDefaultTokenProviders();
            services.AddSingleton<IApiDescriptionGroupCollectionProvider, ApiDescriptionGroupCollectionProvider>();
            services.AddTransient<IApiDescriptionProvider, DefaultApiDescriptionProvider>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<ICategoryFinder, CategoryFinder>()
                    .AddScoped<IHostManager, HostManager>()
                    .AddScoped<ITextTemplateManager, TextTemplateManager>()
                    .AddScoped<ISegmentManager, SegmentManager>()
                    .AddSingleton<IErrorLogger, ErrorLogger>()
                    .AddSingleton<IRedisCache, RedisCache>()
                    .AddScoped<IActionTypeManager, ActionTypeManager>()
                    .AddScoped<IAudienceTypeManager, AudienceTypeManager>()
                    .AddScoped<IHostDataExtractor, HostDataExtractor>()
                    .AddRazorPages();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Dashboard API",
                    Description = "For managing segments and websites",
                    TermsOfService = new Uri("https://example.com/terms"),
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
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
                app.UseSwagger(c =>
    {
        c.SerializeAsV2 = true;
    });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sticky Dashborad API V1");
            });
            app.UseAuthentication();
            app.UseHttpsRedirection();
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
