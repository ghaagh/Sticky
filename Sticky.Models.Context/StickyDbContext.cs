using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ApiRoles = Sticky.Models.Etc.Roles;
namespace Sticky.Models.Context
{
    public partial class StickyDbContext : IdentityDbContext<User>
    {
        public StickyDbContext(DbContextOptions<StickyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActionType> ActionTypes { get; set; }
        public virtual DbSet<ActivityType> ActivityTypes { get; set; }
        public virtual DbSet<AudienceType> AudienceTypes { get; set; }
        public virtual DbSet<CategoryStat> CategoryStats { get; set; }
        public virtual DbSet<Click> Clicks { get; set; }
        public virtual DbSet<Segment> Segments { get; set; }
        public virtual DbSet<Host> Hosts { get; set; }
        public virtual DbSet<PartnerRequestLog> PartnerRequestLogs { get; set; }
        public virtual DbSet<Partner> Partners { get; set; }
        public virtual DbSet<ProductTextTemplate> ProductTextTemplates { get; set; }
        public virtual DbSet<RecordedCategory> RecordedCategories { get; set; }
        public virtual DbSet<ResponseLogger> ResponseLogger { get; set; }
        public virtual DbSet<SegmentPagePattern> SegmentPagePattern { get; set; }
        public virtual DbSet<SegmentStaticNative> SegmentStaticNatives { get; set; }
        public virtual DbSet<SegmentStat> SegmentStats { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<UserTotalVisit> UserTotalVisits { get; set; }
        public virtual DbSet<UsersHostAccess> UsersHostAccess { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "1",Name= ApiRoles.Admin,NormalizedName=ApiRoles.Admin });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2", Name = ApiRoles.HostOwner, NormalizedName = ApiRoles.HostOwner });
            
            modelBuilder.Entity<AudienceType>().HasData(new AudienceType { Id = 1, AudienceTypeName = "بازدید از صفحه" });
            modelBuilder.Entity<AudienceType>().HasData(new AudienceType { Id = 2, AudienceTypeName = "بازدید از محصول" });
            modelBuilder.Entity<AudienceType>().HasData(new AudienceType { Id = 3, AudienceTypeName = "افزودن به سبد" });
            modelBuilder.Entity<AudienceType>().HasData(new AudienceType { Id = 4, AudienceTypeName = "بازدید از کتگوری" });
            modelBuilder.Entity<AudienceType>().HasData(new AudienceType { Id = 5, AudienceTypeName = "خرید" });
            modelBuilder.Entity<AudienceType>().HasData(new AudienceType { Id = 6, AudienceTypeName = "افزودن به علاقه مندی ها" });
            
            modelBuilder.Entity<ActionType>().HasData(new ActionType { Id = 1, Name = "فقط ذخیره کن" });
            modelBuilder.Entity<ActionType>().HasData(new ActionType { Id = 2, Name = "همان محصولات" });
            modelBuilder.Entity<ActionType>().HasData(new ActionType { Id = 3, Name = "کتگوری" });


        }
    }
}
