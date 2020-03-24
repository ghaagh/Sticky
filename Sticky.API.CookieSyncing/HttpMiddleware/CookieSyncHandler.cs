using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sticky.Models.Etc;
using Sticky.Models.Mongo;
using Sticky.Repositories.Advertisement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.API.CookieSyncing.HttpMiddleware
{
    public class CookieSyncHandler
    {
        private readonly IMongoClient _mongoClient;
        private readonly IPartnerCache _partnerCache;
        private readonly IUserIdSetter _userIdSetter;
        private readonly CookieSyncingAPISetting _configuration;
        private readonly ICrowlerCache _crowlerCache;
        public CookieSyncHandler(RequestDelegate next, IUserIdSetter userIdSetter,ICrowlerCache crowlerCache,IPartnerCache partnerCache, IOptions<CookieSyncingAPISetting> configuration,IMongoClient mongoClient)
        {
            _partnerCache = partnerCache;
            _crowlerCache = crowlerCache;
            _mongoClient = mongoClient;
            _userIdSetter = userIdSetter;
            _configuration = configuration.Value;
        }
        public async Task Invoke(HttpContext context)
        {

            var requestHost = context.Request.Headers["Referer"];
            if (string.IsNullOrEmpty(requestHost))
            {
                await context.Response.WriteAsync("No Iframe!");
                return;
            }
            var errorLocation = "";
            try
            {
                var partnerHashCode = context.Request.Query["p"].ToString();
                var partnerUserId = context.Request.Query["pu"].ToString();
                if (string.IsNullOrEmpty(partnerHashCode) || string.IsNullOrEmpty(partnerUserId))
                {
                await context.Response.WriteAsync("<!DOCTYPE html><html><head><meta charset=\"utf-8\" /><title></title></head><body><h1></h1></body></html>");
                return;
                }

                var partnerData = await _partnerCache.FindPartner(partnerHashCode);
                string isNewlySet = context.Request.Cookies["stny"];
                if (isNewlySet != null)
                {
                    return;
                }

                if (partnerData == null)
                    return;
                context.Response.ContentType = "text/html";
                long userId = 0;
                //partner has no cookie from sticky and we need to set cookie if it does not exist and then save it to cookie sync table.
                string userCookie =  context.Request.Cookies[_configuration.CookieName];
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
                        context.Response.Cookies.Append(_configuration.CookieName, userId.ToString(), co);
                        context.Response.Cookies.Append("stny", "1", new CookieOptions() { Domain = _configuration.CookieDomain, SameSite = SameSiteMode.None, Expires = DateTime.Now.AddMinutes(10) });
                    
                }
                    else
                    {
                    context.Response.Cookies.Append("stny", "1", new CookieOptions() { Domain = _configuration.CookieDomain, SameSite = SameSiteMode.None, Expires = DateTime.Now.AddMinutes(10) });
                    var resultofparseCookie = long.TryParse(userCookie, out userId);
                        if (!resultofparseCookie)
                            return;

                    }
                if (await _crowlerCache.IsCrowler(userId))
                {
                    return;
                }
                var partnerId = partnerData.Id;
                IMongoDatabase _database = _mongoClient.GetDatabase("TrackEm");
                IMongoCollection<PartnersCookieMatch> CookieMatches = _database.GetCollection<PartnersCookieMatch>(name: "Partner_" + partnerId + "_CookieMatch", settings: new MongoCollectionSettings() { AssignIdOnInsert = true });
                var filter_Builder = Builders<PartnersCookieMatch>.Filter;
                var filter = filter_Builder.Eq(c => c.Id, partnerUserId);

                var updates = new List<UpdateDefinition<PartnersCookieMatch>>
                {
                    Builders<PartnersCookieMatch>.Update.Set(m => m.StickyId, userId),
                    Builders<PartnersCookieMatch>.Update.SetOnInsert(m => m.Id, partnerUserId)
                };
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                await CookieMatches.UpdateOneAsync(filter, Builders<PartnersCookieMatch>.Update.Combine(updates), new UpdateOptions() { IsUpsert = true }).ConfigureAwait(continueOnCapturedContext: false);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                var rawhtml = "<!DOCTYPE html><html><head><meta charset=\"utf-8\" /><title></title></head><body>Done!</body></html>";
                await context.Response.WriteAsync(rawhtml);
            }
            catch (Exception ex)
            {
                await context.Response.WriteAsync("<script>console.log('" + ex.Message +"on=>" +errorLocation+"')</script>");
            }


        }
    }
}
