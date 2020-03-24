namespace Sticky.Dto.Advertisement.Request
{
    public class UserMembershipsRequest
    {
        public long? StickyUserId { get; set; }
        public string PartnerUserId { get; set; }
        public string Size { get; set; }
        public string PartnerHashCode { get; set; }
    }
}
