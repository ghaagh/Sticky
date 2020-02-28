using System;
using System.Collections.Generic;

namespace Sticky.Models.Context
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaims>();
            AspNetUserLogins = new HashSet<AspNetUserLogins>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            CreatedSegments = new HashSet<CreatedSegments>();
            DruidSegmentOwnership = new HashSet<SegmentOwnership>();
            DruidSegments = new HashSet<Segments>();
            DynamicAdHtml = new HashSet<DynamicAdHtml>();
            Hosts = new HashSet<Hosts>();
            ProductGroupings = new HashSet<ProductGroupings>();
            SegmentPagePattern = new HashSet<SegmentPagePattern>();
            UsersHostAccess = new HashSet<UsersHostAccess>();
        }

        public string Id { get; set; }
        public int AccessFailedCount { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string NormalizedEmail { get; set; }
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string UserName { get; set; }
        public int? HostId { get; set; }
        public int? PartnerId { get; set; }
        public bool Validated { get; set; }

        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual ICollection<SegmentOwnership> DruidSegmentOwnership { get; set; }
        public virtual ICollection<Segments> DruidSegments { get; set; }
        public virtual ICollection<DynamicAdHtml> DynamicAdHtml { get; set; }
        public virtual ICollection<Hosts> Hosts { get; set; }
        public virtual ICollection<ProductGroupings> ProductGroupings { get; set; }
        public virtual ICollection<SegmentPagePattern> SegmentPagePattern { get; set; }
        public virtual ICollection<UsersHostAccess> UsersHostAccess { get; set; }
    }
}
