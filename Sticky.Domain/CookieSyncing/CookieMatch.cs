namespace Sticky.Domain.CookieSyncing
{
    public class CookieMatch
    {
        public CookieMatch()
        {

        }

        public CookieMatch(string partnerUserId, long partnerId, long stickyId)
        {
            Id = partnerUserId;
            PartnerId = partnerId;
            StickyId = stickyId;
        }

        public string Id { get; set; }
        public long PartnerId { get; set; }
        public long StickyId { get; set; }
    }
}
