using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sticky.Models.Etc;
using Sticky.Repositories.Advertisement;
using Sticky.Repositories.Common;
using System;
using System.Threading.Tasks;

namespace Sticky.API.Script.HttpMiddleware
{
    //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class IFrameHandler
    {
        private readonly IHostCache _hostCache;
        private readonly ICrowlerCache _crowlerCache;
        private readonly IPartnerCache _partnerCache;
        private readonly IUserIdSetter _userIdSetter;
        private readonly IErrorLogger _errorLogger;
        private readonly ScriptAPISetting _configuration;
        public IFrameHandler(RequestDelegate next, IUserIdSetter userIdSetter,ICrowlerCache crowlerCache,IErrorLogger errorLogger,IOptions<ScriptAPISetting> configuration,IHostCache hostDictionary, IPartnerCache partnersManager)
        {
            _partnerCache = partnersManager;
            _hostCache = hostDictionary;
            _userIdSetter = userIdSetter;
            _crowlerCache = crowlerCache;
           _errorLogger = errorLogger;
            _configuration = configuration.Value;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
              
            context.Response.ContentType = "text/html";
            var requestHost = context.Request.Headers["Referer"];
            if(string.IsNullOrEmpty(requestHost))
            {
                await context.Response.WriteAsync("No Iframe! Don't Cheat!");
                return;
            }
            string host = new Uri(requestHost).Authority.ToString();
                var topdomain = "";
                if (host.IndexOf(".") == host.LastIndexOf("."))
                    topdomain = host;
                else
                    topdomain = host.Substring(host.IndexOf(".") + 1);
                var hostData =await  _hostCache.GetHostAsync(topdomain);
            if (hostData==null)
                return;

            string userCookie = context.Request.Cookies[_configuration.CookieName];
                //try
                //{
                //    var logAgent= _redisCache.GetDatabase(DataAccess.RedisDatabases.CacheData).StringGet("LogAgent");
                //    if (logAgent.HasValue)
                //    {
                //    var userAgent = context.Request.Headers["User-Agent"].ToString();
                //    var agentstring = string.IsNullOrEmpty(userAgent.ToString()) ? "unknown" : userAgent.ToString();
                //    var agentDb = _redisCache.GetDatabase(DataAccess.RedisDatabases.Logs);
                //    await agentDb.HashIncrementAsync("AgentCounter",agentstring);
                //    }

                //}
                //catch
                //{

                //}

                long userId = 0;
            if (userCookie == null)
            {
                userId = await _userIdSetter.GetNewUserIdAsync();
                var co = new CookieOptions()
                {
                    Expires = DateTime.Now.AddYears(5),
                    SameSite = SameSiteMode.None

                    
                };
                if (_configuration.CookieDomain != ".")
                    co.Domain = _configuration.CookieDomain;
                context.Response.Cookies.Append(_configuration.CookieName, userId.ToString(),co);
            }
            else
            {
                var resultofparseCookie = long.TryParse(userCookie, out userId);
                if (!resultofparseCookie)
                    return;

            }
                if (await _crowlerCache.IsCrowler(userId))
                {
                    return;
                }
                var rawhtml = "<!DOCTYPE html><html><head><meta charset=\"utf-8\" /><title></title></head><body>partner_iframes<script>var sendingData = {};sendingData.message = \"GetCookie\";sendingData.UserId = user_identity;sendingData.FinalizePage = 'finalize_page';sendingData.HostId = host_identity;sendingData.AddToCart = 'add_to_cart';parent.postMessage(JSON.stringify(sendingData), \"*\");</script></body></html>";
            var html = rawhtml.Replace("user_identity", userId.ToString()).Replace("host_identity", hostData.Id.ToString()).Replace("add_to_cart",hostData.AddToCardId).Replace("finalize_page",hostData.FinalizePage);
                var partners = await _partnerCache.ListAsync();
                var pixelHtml = "";
                foreach (var item in partners)
                {
                    if (!string.IsNullOrEmpty(item.CookieSyncAddress))
                    {
                    var partnerAddress = item.CookieSyncAddress?.Replace("@StickyId", userId.ToString());
                    pixelHtml+="<iframe src=\""+partnerAddress+"\"></iframe>";
                    }

                }
               html= html.Replace("partner_iframes", pixelHtml);
                await context.Response.WriteAsync(html);
            }
            catch(Exception ex)
            {
               await  _errorLogger.LogError(ex.Message);
            }


        }
    }
}
