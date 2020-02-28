using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class AspNetUserRoles
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
