using Microsoft.AspNetCore.Builder;
using Sticky.Application.Dashboard.Middlewares.ExceptionHandling;

namespace Sticky.Application.Dashboard.Extensions.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}