
using Newtonsoft.Json;

namespace Sticky.Infrastructure.Cache.Models
{
    public class HostCache : CacheModel
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string Host { get; set; }
        public string HashCode { get; set; }
        public bool HostValidated { get; set; }
        public bool PageValidated { get; set; }
        public bool ProductValidated { get; set; }
        public bool CategoryValidated { get; set; }
        public bool AddToCardValidated { get; set; }
        public bool FinalizeValidated { get; set; }
        public string ValidatingHtmlAddress { get; set; }
        public string AddToCardId { get; set; }
        public string FinalizePage { get; set; }
        public bool? HostPriority { get; set; }
        public string LogoAddress { get; set; }
        public int? ProductImageWidth { get; set; }
        public int? ProductImageHeight { get; set; }
        public string LogoOtherData { get; set; }

        [JsonIgnore]
        public string Key
        {
            get {
                return Host;
            }
        }
    }
}
