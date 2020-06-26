using Microsoft.AspNetCore.Builder;

namespace Sticky.API.Script.HttpMiddleware
{
    public static class IframeMiddlewareExtension
    {
        public static IApplicationBuilder UseIframeMiddlewareExtension
                                      (this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IFrameHandler>();
        }

    }


}
