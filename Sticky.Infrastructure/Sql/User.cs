using Microsoft.AspNetCore.Identity;
using Sticky.Domain.HostAggrigate;
using Sticky.Domain.UserAggrigate;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sticky.Infrastructure.Sql
{
    public class User : IdentityUser, IIdentity
    {
        public User()
        {

        }
        private HashSet<UsersHostAccess> _host;
        public virtual IReadOnlyCollection<UsersHostAccess> UsersHostAccesses {
            get
            {
                return _host;
            }
            set
            {
                _host =(HashSet<UsersHostAccess>)value;
            }
        }

        public string GetUserId() => Id;
    }
}
