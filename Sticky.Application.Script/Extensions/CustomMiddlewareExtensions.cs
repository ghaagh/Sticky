using Microsoft.AspNetCore.Builder;
using Sticky.Application.Script.Middlewares;

namespace Sticky.Application.Script.Extensions
{
    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomHanlderMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IFrameHandler>();
        }

    }


}
