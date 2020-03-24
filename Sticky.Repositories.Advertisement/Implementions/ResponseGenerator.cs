using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sticky.Dto.Advertisement.Response;
using Sticky.Models.Etc;
using Sticky.Models.Redis;
using Sticky.Repositories.Common;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public class ResponseGenerator : IResponseGenerator
    {
        private readonly AdvertisementAPISetting _setting;
        private readonly IEncodeDecodeManager _encodeDecodeManager;

        private readonly IAwesomeTextGenerator _awesomeTextGenerator;
        public ResponseGenerator(IOptions<AdvertisementAPISetting> options, IEncodeDecodeManager encodeDecodeManager,IAwesomeTextGenerator awesomeTextGenerator)
        {
            _awesomeTextGenerator = awesomeTextGenerator;
            _encodeDecodeManager = encodeDecodeManager;
            _setting = options.Value;
        }
        public async Task<MembershipResponse> CreateResponseAsync(long userId, int partnerId,  List<UserSegment> segments)
        {
            List<MemberShipResult> results = new List<MemberShipResult>();
            foreach(var item in segments)
            {
                var newMembershipRow = new MemberShipResult
                {
                    SegmentId = item.SegmentId,
                    HostId = item.HostId,
                    Priority = item.SegmentPriority
                };

                foreach (var c in item.Products)
                {
                    var advertisingTextData = await _awesomeTextGenerator.CreateAdvertisingText(item.SegmentId, c.ProductName, c.Price);
                    if (c.ImageAddress != null)
                    {
                    newMembershipRow.Products.Add(new NativeDetails()
                    {
                        AdId =  _encodeDecodeManager.Base64Encode(item.SegmentId + "$$$" + advertisingTextData.TemplateText),
                        Image = c.ImageAddress?.Replace("m_lfit,h_350,w_350", "resize,m_pad,h_330,w_500,color_FFFFFF").Replace("m_lfit,h_500,w_500", "resize,m_pad,h_330,w_500,color_FFFFFF").
                        Replace("m_lfit,h_600,w_600", "resize,m_pad,h_330,w_500,color_FFFFFF"),
                        Price = c.Price,
                        ProductId =c.Id,
                        OldPrice = c.Price,
                        OriginalProductName =_awesomeTextGenerator.Clean(c.ProductName),
                        ProductName =advertisingTextData.ProductText ,
                        UrlAddress = $"{_setting.AdvertisementUrlBase}Click?landing={_encodeDecodeManager.Base64Encode(c.Url??"")}&segmentId={item.SegmentId}&stpd={c.Id}&uadid={_encodeDecodeManager.Base64Encode(item.SegmentId + "$$$" + advertisingTextData.TemplateText)}"
                    }
                    );
                    }

                }
                results.Add(newMembershipRow);
                
            }
            return new MembershipResponse() {
                Status = ResponseStatus.Success,
                UserId = userId,
                Result = results,
            };  

        }
        public MembershipResponse EmptyResponse(string status, string message)
        {
            return new MembershipResponse() { Message = message, Status = status };
        }
    }
}
