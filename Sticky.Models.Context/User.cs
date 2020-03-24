using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public class User:IdentityUser
    {
        public User()
        {
            Hosts = new HashSet<Host>();
            Segments = new HashSet<Segment>();
            UsersHostAccesses = new HashSet<UsersHostAccess>();
        }
        public virtual ICollection<Segment> Segments { get; set; }
        public virtual ICollection<Host> Hosts { get; set; }
        public virtual ICollection<UsersHostAccess> UsersHostAccesses { get; set; }
    }
}
