using Microsoft.Extensions.Options;
using Sticky.Domain.CookieSyncing;
using Sticky.Domain.ResponseAggrigate;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Application.Advertising.Services
{
    class MembershipFinder : IMembershipFinder
    {
        private readonly ICookieSyncRepository _cookieSyncRepository;
        private readonly IResponseRepositoy _responseRepositoy;
        private readonly IEncodeDecodeManager _encodeDecodeManager;
        private readonly IRequestRepository _requestRepository;
        private readonly IAdTextGenerator _adTextGenerator;
        private readonly Setting _setting;
        public MembershipFinder(ICookieSyncRepository cookieSyncRepository,IOptions<Setting> options, IEncodeDecodeManager encodeDecodeManager, IAdTextGenerator adTextGenerator, IResponseRepositoy responseRepositoy, IRequestRepository requestRepository)
        {
            _cookieSyncRepository = cookieSyncRepository;
            _responseRepositoy = responseRepositoy;
            _requestRepository = requestRepository;
            _adTextGenerator = adTextGenerator;
            _encodeDecodeManager = encodeDecodeManager;
            _setting = options.Value;
        }

        public async Task<IEnumerable<Membership>> GetByPartnerUserIdAsync(string partnerHash, string partnerUserId)
        {
            var stickyUserId = await _cookieSyncRepository.GetStickyCookie(partnerHash, partnerUserId);
            if (stickyUserId == 0)
                return new List<Membership>();
            return await GetByStickyUserIdAsync(stickyUserId);
        }

        public async Task<IEnumerable<Membership>> GetByStickyUserIdAsync(long stickyId)
        {
            List<Membership> memberships = new List<Membership>();
            var general = await _responseRepositoy.GetMembership(ResponseUpdaterTypeEnum.ProductAndPage, stickyId);
            if (general.Any())
                memberships.AddRange(general);
            else
                await _requestRepository.EnqueRequest(stickyId, ResponseUpdaterTypeEnum.ProductAndPage);

            var category = await _responseRepositoy.GetMembership(ResponseUpdaterTypeEnum.Category, stickyId);
            if (category.Any())
                memberships.AddRange(category);
            else
                await _requestRepository.EnqueRequest(stickyId, ResponseUpdaterTypeEnum.Category);

            var excluded = await _responseRepositoy.GetMembership(ResponseUpdaterTypeEnum.SpecialSegment, stickyId);
            if (excluded.Any())
                memberships.AddRange(excluded);
            else
                await _requestRepository.EnqueRequest(stickyId, ResponseUpdaterTypeEnum.SpecialSegment);

            if(!memberships.Any())
                return memberships;
            else
            {
                var adReadyMembership = new List<Membership>();
                foreach (var item in memberships)
                {
                    var adReadyMembershipItem = new Membership() { 
                        SegmentId= item.SegmentId,
                        HostId= item.HostId
                    };
                    foreach (var c in item.Products)
                    {
                        var advertisingTextData = await _adTextGenerator.CreateAdvertisingText(item.SegmentId, c.ProductName, c.Price);
                        if (c.Image != null)
                        {
                            adReadyMembershipItem.Products.Add(new MemberShipProduct()
                            {
                                AdId = _encodeDecodeManager.Base64Encode(item.SegmentId + "$$$" + advertisingTextData.templateId),
                                Image = c.Image,
                                Price = c.Price,
                                ProductId = c.ProductId,
                                OldPrice = c.Price,
                                OriginalProductName = _adTextGenerator.Clean(c.ProductName),
                                ProductName = advertisingTextData.finalName,
                                UrlAddress = $"{_setting.UrlBase}Click?landing={_encodeDecodeManager.Base64Encode(c.UrlAddress ?? "")}&segmentId={item.SegmentId}&stpd={c.ProductId}&uadid={_encodeDecodeManager.Base64Encode(item.SegmentId + "$$$" + advertisingTextData.templateId)}"
                            }
                            );
                        }
                    }
                    adReadyMembership.Add(adReadyMembershipItem);

                }
                return adReadyMembership;
            }
        }
    }
}
