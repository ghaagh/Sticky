using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sticky.Domain.ClientUsers;
using Sticky.Infrastructure.Cache;
using Sticky.Infrastructure.Cache.Models;
using Sticky.Application.Script.Services;
using System;
using System.Threading.Tasks;

namespace Sticky.Application.Script.Middlewares
{
    public class IFrameHandler
    {
        private const string rawHtml = @"
                                <!DOCTYPE html>
                                <html>
                                   <head>
                                      <meta charset='utf-8' />
                                      <title></title>
                                   </head>
                                   <body>
                                      partner_iframes
                                      <script> let sendingData = {};
                                         sendingData.message = 'GetCookie';
                                         sendingData.UserId = user_identity;
                                         sendingData.FinalizePage = 'finalize_page';
                                         sendingData.HostId = host_identity;
                                         sendingData.AddToCart = 'add_to_cart';
                                         parent.postMessage(JSON.stringify(sendingData), '*');
                                      </script>
                                   </body>
                                </html>
                                ";
        private readonly ICache<HostCache> _hostCache;
        private readonly IMultipleCache<PartnerCache> _partnerCache;
        private readonly IClientUserRepository _clientUserRepository;
        private readonly IUtility _domainExtractor;
        private readonly Setting _setting;
        public IFrameHandler(RequestDelegate next, IUtility domainExtractor, IClientUserRepository clientUserRepository, IOptions<Setting> options, ICache<HostCache> hostCache, IMultipleCache<PartnerCache> partnerCache)
        {
            _partnerCache = partnerCache;
            _hostCache = hostCache;
            _domainExtractor = domainExtractor;
            _clientUserRepository = clientUserRepository;
            _setting = options.Value;
        }
        public async Task Invoke(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            var requestHost = context.Request.Headers["Referer"];
            if (string.IsNullOrEmpty(requestHost))
            {
                await context.Response.WriteAsync("No Iframe!");
                return;
            }
            var host = _domainExtractor.ExtractDomain(requestHost);
            var hostData = await _hostCache.GetAsync(host);
            if (hostData == null)
                return;

            string userCookie = context.Request.Cookies[_setting.CookieName];

            long userId;
            if (userCookie == null)
            {
                userId = await _clientUserRepository.CreateAsync();
                var co = new CookieOptions()
                {
                    Expires = DateTime.Now.AddYears(5),
                    SameSite = SameSiteMode.None


                };
                if (_setting.CookieDomain != ".")
                    co.Domain = _setting.CookieDomain;
                context.Response.Cookies.Append(_setting.CookieName, userId.ToString(), co);
            }
            else
            {
                var resultofparseCookie = long.TryParse(userCookie, out userId);
                if (!resultofparseCookie)
                    return;
            }
            var html = rawHtml.
                Replace("user_identity", userId.ToString())
                .Replace("host_identity", hostData.Id.ToString())
                .Replace("add_to_cart", hostData.AddToCardId)
                .Replace("finalize_page", hostData.FinalizePage);

            var partners = await _partnerCache.GetListAsync();
            var pixelHtml = string.Empty;
            foreach (var item in partners)
            {
                if (!string.IsNullOrEmpty(item.CookieSyncAddress))
                {
                    var partnerAddress = item.CookieSyncAddress?
                        .Replace("@StickyId", userId.ToString());
                    pixelHtml += "<iframe src=\"" + partnerAddress + "\"></iframe>";
                }

            }
            html = html.Replace("partner_iframes", pixelHtml);
            await context.Response.WriteAsync(html);
        }
    }
}
