using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sticky.Dto.Script.Request;
using Sticky.Models.Druid;
using Sticky.Models.Etc;
using Sticky.Repositories.Advertisement;
using Sticky.Repositories.Script;
using Sticky.Repositories.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace Sticky.API.Script.Controllers
{
    [Produces("application/json")]
    public class ActionLoggerController : Controller
    {
        private readonly IHostCache _hostCache;
        private readonly IUtility _utility;
        private readonly IKafkaLogProducer _kafkaLogger;
        private readonly ICrowlerCache _crowlerCache;
        private readonly ITotalVisitUpdater _totalVisitUpdater;
        private readonly ScriptAPISetting _configurations;
        private readonly IProductCache _productCache;
        private readonly IHostScriptChecker _hostScriptChecker;
        public ActionLoggerController(IOptions<ScriptAPISetting> conf,
            IHostScriptChecker hostScriptChecker,
            ICrowlerCache crowlerCache,
            IKafkaLogProducer kafkaLogger,
            IUtility utility,
            IProductCache productCache,
            ITotalVisitUpdater totalVisitUpdater,
            IHostCache hostCache)
        {
            _crowlerCache = crowlerCache;
            _kafkaLogger = kafkaLogger;
            _utility = utility;
            _hostScriptChecker = hostScriptChecker;
            _hostCache = hostCache;
            _productCache = productCache;
            _totalVisitUpdater = totalVisitUpdater;
            _configurations = conf.Value;

        }
        private bool HostNotExists(Models.Redis.HostWithoutRelation host)
        {
            return (host == null || host.Id == 0);
        }
        private bool DomainCheck(string origin, string host)
        {
            if (!_configurations.SecurityCheck)
                return true;
            if (!string.IsNullOrEmpty(origin))
            {
                var topDomain = _utility.GetTopDomainFromAddress(origin);
                if (host != topDomain)
                    return false;
            }
            return true;
        }
        [HttpPost("AdToCart")]
        public async Task<string> AddToCart([FromBody]ModifyProductLog logData)
        {
            var host = await _hostCache.GetHostByIdAsync(logData.HostId);
            if (HostNotExists(host))
                return CommonStrings.NoHost;
            if (!DomainCheck(Request.Headers[CommonStrings.Origin].ToString(), host.Host))
                return CommonStrings.NoHostAccess;
            await _hostScriptChecker.UpdateCartValidation(logData.HostId);
            await _totalVisitUpdater.UpdateTotalVisit(logData.HostId);
            await _kafkaLogger.GenerateProductLogFromId(StatTypes.AddToCart, logData, host.Host);
            return string.Empty;
        }
        [HttpPost("RemoveCart")]
        public async Task<string> RemoveFromCard([FromBody]ModifyProductLog logData)
        {
            var host = await _hostCache.GetHostByIdAsync(logData.HostId);
            if (HostNotExists(host))
                return CommonStrings.NoHost;
            if (!DomainCheck(Request.Headers[CommonStrings.Origin].ToString(), host.Host))
                return CommonStrings.NoHostAccess;
            await _hostScriptChecker.UpdateCartValidation(logData.HostId);
            await _totalVisitUpdater.UpdateTotalVisit(logData.HostId);
            await _kafkaLogger.GenerateProductLogFromId(StatTypes.RemoveFromCart, logData, host.Host);
            return string.Empty;
        }
        [HttpPost("Like")]
        public async Task<string> LikeProducts([FromBody]ModifyProductLog logData)
        {
            var host = await _hostCache.GetHostByIdAsync(logData.HostId);
            if (HostNotExists(host))
                return CommonStrings.NoHost;
            if (!DomainCheck(Request.Headers[CommonStrings.Origin].ToString(), host.Host))
                return CommonStrings.NoHostAccess;
            await _hostScriptChecker.UpdateFavValidation(logData.HostId);
            await _totalVisitUpdater.UpdateTotalVisit(logData.HostId);
            await _kafkaLogger.GenerateProductLogFromId(StatTypes.Like, logData, host.Host);
            return string.Empty;
        }
        [HttpPost("Unlike")]
        public async Task<string> UnLikeProducts([FromBody]ModifyProductLog logData)
        {
            var host = await _hostCache.GetHostByIdAsync(logData.HostId);
            if (HostNotExists(host))
                return CommonStrings.NoHost;
            if (!DomainCheck(Request.Headers[CommonStrings.Origin].ToString(), host.Host))
                return CommonStrings.NoHostAccess;
            await _hostScriptChecker.UpdateFavValidation(logData.HostId);
            await _totalVisitUpdater.UpdateTotalVisit(logData.HostId);
            await _kafkaLogger.GenerateProductLogFromId(StatTypes.Unlike, logData, host.Host);
            return string.Empty;
        }
        [HttpPost("BuyCart")]
        public async Task<string> BuyCart([FromBody]ModifyProductLog logData)
        {
            var host = await _hostCache.GetHostByIdAsync(logData.HostId);
            if (HostNotExists(host))
                return CommonStrings.NoHost;
            if (!DomainCheck(Request.Headers[CommonStrings.Origin].ToString(), host.Host))
                return CommonStrings.NoHostAccess;
            await _hostScriptChecker.UpdateBuyValidation(logData.HostId);
            await _totalVisitUpdater.UpdateTotalVisit(logData.HostId);
            await _kafkaLogger.GenerateProductLogFromId(StatTypes.ProductPurchase, logData, host.Host);
            return string.Empty;
        }
        [HttpPost("ProductUpdate")]
        public async Task<string> ProductViewAndUpdate([FromBody]ProductLog logData)
        {
            var host = await _hostCache.GetHostByIdAsync(logData.HostId);
            if (HostNotExists(host))
                return CommonStrings.NoHost;
            if (!DomainCheck(Request.Headers[CommonStrings.Origin].ToString(), host.Host))
                return CommonStrings.NoHostAccess;
            if (logData != null && (await _crowlerCache.IsCrowler(logData.UserId)))
                return string.Empty;
            await _hostScriptChecker.UpdateProductValidation(logData.HostId);
            await _totalVisitUpdater.UpdateTotalVisit(logData.HostId);
            await _kafkaLogger.GenerateProductLogFromProducts(StatTypes.ProductView, logData, host.Host);
            return string.Empty;
        }
        [HttpPost("PageLogger")]
        public async Task<string> PageVisitLogger([FromBody]PageLogRequest pageData)
        {
            var host = await _hostCache.GetHostByIdAsync(pageData.HostId);
            if (host == null || host.Id == 0)
                return CommonStrings.NoHost;
            if (pageData != null && (await _crowlerCache.IsCrowler(pageData.UserId)))
                return string.Empty;
            await _totalVisitUpdater.UpdateTotalVisit(pageData.HostId);
            await _hostScriptChecker.UpdatePageValidation(pageData.HostId);
            await _kafkaLogger.GeneratePageLog(pageData, host.Host);
            return string.Empty;
        }
    }
}