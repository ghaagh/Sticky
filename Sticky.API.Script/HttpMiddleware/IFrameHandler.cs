using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sticky.Models.Etc;
using Sticky.Repositories.Advertisement;
using Sticky.Repositories.Common;
using System;
using System.Threading.Tasks;

namespace Sticky.API.Script.HttpMiddleware
{
    public class IFrameHandler
    {
        private readonly IHostCache _hostCache;
        private readonly ICrowlerCache _crowlerCache;
        private readonly IPartnerCache _partnerCache;
        private readonly IUserIdSetter _userIdSetter;
        private readonly IUtility _utility;
        private readonly ScriptAPISetting _configuration;
        public IFrameHandler(RequestDelegate next,
            IUserIdSetter userIdSetter,
            IUtility utility,
            ICrowlerCache crowlerCache,
            IOptions<ScriptAPISetting> configuration,
            IHostCache hostDictionary,
            IPartnerCache partnersManager)
        {
            _utility = utility;
            _partnerCache = partnersManager;
            _hostCache = hostDictionary;
            _userIdSetter = userIdSetter;
            _crowlerCache = crowlerCache;
            _configuration = configuration.Value;
        }
        public async Task Invoke(HttpContext context)
        {
            context.Response.ContentType = CommonStrings.ContentTypeTextHtml;
            var requestHost = GetReferrer(context);
            if (ReferrerIsEmpty(requestHost))
                return;
            var topDomain = _utility.GetTopDomainFromAddress(requestHost);
            var host = await _hostCache.GetHostAsync(topDomain);
            if (host == null)
                return;
            string userCookie = context.Request.Cookies[_configuration.CookieName];
            long userId;
            if (userCookie == null)
                userId = await SetNewUserCookieAsync(context);
            else
            {
                var resultofparseCookie = long.TryParse(userCookie, out userId);
                if (!resultofparseCookie)
                    return;
            }
            if (await _crowlerCache.IsCrowler(userId))
                return;
            var html = await CreateInitialResponseAsync(userId, host);
            await context.Response.WriteAsync(html);
        }
        private string GetReferrer(HttpContext context)
        {
            return context.Request.Headers[CommonStrings.ReferrerHeader];
        }
        private bool ReferrerIsEmpty(string referrer)
        {
            return string.IsNullOrEmpty(referrer);
        }

        private async Task<long> SetNewUserCookieAsync(HttpContext context)
        {
            long newUserId = await _userIdSetter.GetNewUserIdAsync();
            var co = new CookieOptions
            {
                Expires = DateTime.Now.AddYears(5),
                SameSite = SameSiteMode.None
            };
            if (_configuration.CookieDomain != CommonStrings.Dot)
                co.Domain = _configuration.CookieDomain;
            context.Response.Cookies.Append(_configuration.CookieName, newUserId.ToString(), co);
            return newUserId;

        }
        private async Task<string> GeneratePartnerPixelsAsync(long userId)
        {
            var pixelHtml = "";
            var partners = await _partnerCache.ListAsync();
            foreach (var item in partners)
            {
                if (!string.IsNullOrEmpty(item.CookieSyncAddress))
                {
                    var partnerAddress = item.CookieSyncAddress?.Replace("@StickyId", userId.ToString());
                    pixelHtml += "<iframe src=\"" + partnerAddress + "\"></iframe>";
                }

            }
            return pixelHtml;
        }
        private async Task<string> CreateInitialResponseAsync(long userId, Models.Redis.HostWithoutRelation host)
        {
            var html = Initial.ResponseString
                .Replace("user_identity", userId.ToString())
                .Replace("host_identity", host.Id.ToString())
                .Replace("add_to_cart", host.AddToCardId)
                .Replace("finalize_page", host.FinalizePage)
                .Replace("partner_iframes", await GeneratePartnerPixelsAsync(userId));
            return html;
        }

    }
}
