using Sticky.Domain.UserAggrigate;

namespace Sticky.Domain.HostAggrigate
{
    public partial class UsersHostAccess
    {
        private UsersHostAccess()
        {

        }
        public UsersHostAccess(string userId,bool adminAccess = false):base()
        {
            UserId = userId;
            AdminAccess = adminAccess;
        }
        public string UserId { get; private set; }
        public long HostId { get; private set; }
        public virtual IIdentity User { get; private set; }
        public virtual Host Host { get; set; }
        public bool AdminAccess { get; private set; }
    }
}
