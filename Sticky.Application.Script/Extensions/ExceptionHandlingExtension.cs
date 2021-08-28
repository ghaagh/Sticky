using Microsoft.AspNetCore.Builder;
using Sticky.Application.Script.Middlewares.ExceptionHandling;

namespace Sticky.Application.Script.Extensions.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}