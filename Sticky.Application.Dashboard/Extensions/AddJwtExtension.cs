using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sticky.Application.Dashboard.Settings;
using System;

using System.Text;

namespace Sticky.Application.Dashboard.Extensions
{
    public static class AddJwtExtension
    {
        public static IServiceCollection AddJWT(this IServiceCollection serviceCollection, Action<JwtSetting> configureJwt)
        {

            serviceCollection.Configure(configureJwt);
            var setting = new JwtSetting();
            configureJwt(setting);
            serviceCollection.AddAuthorization();
            serviceCollection.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidIssuer=setting.Issuer,
                    ValidateIssuerSigningKey = false,
                    RequireExpirationTime=false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            }).AddCookie(option=> {
                option.LoginPath = PathString.Empty;
            });
            return serviceCollection;
        }
    }
}
