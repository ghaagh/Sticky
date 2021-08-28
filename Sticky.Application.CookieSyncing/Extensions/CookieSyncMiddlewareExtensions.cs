using Microsoft.AspNetCore.Builder;
using Sticky.Application.CookieSyncing.Middlewares;

namespace Sticky.Application.CookieSyncing.Extensions
{
    public static class CookieSyncMiddlewareExtensions
    {
        public static IApplicationBuilder UseCookieSyncMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CookieSyncHandler>();
        }

    }


}
