using Microsoft.AspNetCore.Builder;

namespace Sticky.API.Script.HttpMiddleware
{
    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomHanlderMiddleware
                                      (this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IFrameHandler>();
        }

    }


}
