using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sticky.Dto.Script.Request;
using Sticky.Models.Druid;
using Sticky.Models.Etc;
using Sticky.Repositories.Advertisement;
using Sticky.Repositories.Common;
using System;
using System.Threading.Tasks;
namespace Sticky.API.Script.Controllers
{
    [Produces("application/json")]
    [Route("PageLogger")]
    public class PageLoggerController : Controller
    {
        private readonly ScriptAPISetting _configurations;
        private readonly IHostCache _hostCache;
        private readonly IErrorLogger _errorLogger;
        private readonly ICrowlerCache _crowlerCache;
        private readonly IKafkaLogger _kafkaLogger;
        private readonly IHostScriptChecker _hostScriptChecker;
        private readonly ITotalVisitUpdater _totalVisitUpdater;
        public PageLoggerController(IOptions<ScriptAPISetting> conf, IHostScriptChecker hostScriptChecker, IKafkaLogger kafkaLogger, IErrorLogger errorLogger, ICrowlerCache crowlerCache, IHostCache hostDictionary, ITotalVisitUpdater totalVisitUpdater)
        {
            _kafkaLogger = kafkaLogger;
            _hostCache = hostDictionary;
            _hostScriptChecker = hostScriptChecker;
            _crowlerCache = crowlerCache;
            _errorLogger = errorLogger;
            _totalVisitUpdater = totalVisitUpdater;
            _configurations = conf.Value;
        }
        [HttpPost]
        public async Task<string> LogUser([FromBody]PageLogRequest pageData)
        {
            try
            {
                var innerAddress = new Uri(pageData.Address).PathAndQuery;
                var host = await _hostCache.GetHostByIdAsync(pageData.HostId);
                if (host == null || host.Id == 0)
                    return CommonStrings.NoHost;
                #region SecurityCheck
                if (_configurations.SecurityCheck)
                {
                    var origin = Request.Headers[CommonStrings.Origin].ToString();
                    if (!string.IsNullOrEmpty(origin))
                    {
                        var aut = new Uri(origin).Host;
                        aut = aut.ToLower();
                        var topdomain = string.Empty;
                        if (aut.IndexOf(CommonStrings.Dot) == aut.LastIndexOf(CommonStrings.Dot))
                            topdomain = aut;
                        else
                            topdomain = aut.Substring(aut.IndexOf(CommonStrings.Dot) + 1);
                        if (host.Host != topdomain)
                            return CommonStrings.NoHostAccess;
                    }
                }
                #endregion

                if (pageData != null && (await _crowlerCache.IsCrowler(pageData.UserId)))
                    return "";
                await _totalVisitUpdater.UpdateTotalVisit(pageData.HostId);
                await _hostScriptChecker.UpdatePageValidation(pageData.HostId);
                await _kafkaLogger.SendMessage(new DruidData()
                {
                    CategoryName = "",
                    Date = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    HostName = host.Host,
                    ImageAddress = "",
                    Price = 0,
                    ProductName = "",
                    PageAddress = innerAddress,
                    ProductId = "",
                    StatType = StatTypes.PageView,
                    UserId = pageData.UserId.ToString(),
                    HostId = pageData.HostId.ToString(),

                });
         

            }
            catch (Exception ex)
            {
                await _errorLogger.LogError("Operation:PageLogger =>" + "User=>" + pageData.UserId + ":" + ex.Message);
            }
            return "";


        }
    }
}