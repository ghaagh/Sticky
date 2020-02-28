using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class UsersHostAccess
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? HostId { get; set; }
        public bool AdminAccess { get; set; }

        public virtual Host Host { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
