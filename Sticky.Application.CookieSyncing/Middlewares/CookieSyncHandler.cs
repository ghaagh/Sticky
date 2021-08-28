using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sticky.Domain.ClientUsers;
using Sticky.Domain.CookieSyncing;
using Sticky.Infrastructure.Cache;
using Sticky.Infrastructure.Cache.Models;
using System;
using System.Threading.Tasks;

namespace Sticky.Application.CookieSyncing.Middlewares
{
    public class CookieSyncHandler
    {
        private const string successHtml = "<!DOCTYPE html><html><head><meta charset=\"utf-8\" /><title></title></head><body>Done!</body></html>";
        private readonly ICookieSyncRepository _cookieSyncRepository;
        private readonly IMultipleCache<PartnerCache> _partnerCache;
        private readonly IClientUserRepository _clientUserRepository;
        private readonly Setting _setting;
#pragma warning disable IDE0060 // Remove unused parameter
        public CookieSyncHandler(RequestDelegate next, ICookieSyncRepository cookieSyncRepository, IMultipleCache<PartnerCache> partnerCache, IClientUserRepository clientUserRepository, IOptions<Setting> setting)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            _partnerCache = partnerCache;
            _clientUserRepository = clientUserRepository;
            _cookieSyncRepository = cookieSyncRepository;
            _setting = setting.Value;
        }
        public async Task Invoke(HttpContext context)
        {

            context.Response.ContentType = "text/html";
            var requestHost = context.Request.Headers["Referer"];
            if (string.IsNullOrEmpty(requestHost))
                return;

            var partnerId = context.Request.Query["p"].ToString();
            var partnerUserId = context.Request.Query["pu"].ToString();
            if (string.IsNullOrEmpty(partnerId) || string.IsNullOrEmpty(partnerUserId))
                return;

            var partner = await _partnerCache.GetAsync(partnerId);
            if (partner == null)
                return;
            //Means: not call the repository to much. just let it rest for 10 minutes
            string isNewlySet = context.Request.Cookies["stny"];
            if (isNewlySet != null)
            {
                return;
            }
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
                context.Response.Cookies.Append("stny", "1", new CookieOptions() { Domain = _setting.CookieDomain, SameSite = SameSiteMode.None, Expires = DateTime.Now.AddMinutes(10) });

            }
            else
            {
                var resultofparseCookie = long.TryParse(userCookie, out userId);
                if (!resultofparseCookie)
                    return;
            }

            await _cookieSyncRepository.SyncCookieAsync(new CookieMatch(partnerUserId,partner.Id,userId));
            await context.Response.WriteAsync(successHtml);
        }
    }
}
