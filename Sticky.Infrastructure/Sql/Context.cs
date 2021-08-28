using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sticky.Domain.CategoryAggrigate;
using Sticky.Domain.HostAggrigate;
using Sticky.Domain.PartnerAggrigate;
using Sticky.Domain.SegmentAggrigate;

namespace Sticky.Infrastructure.Sql
{
    public class Context : IdentityDbContext<User>
    {
        public Context(DbContextOptions<Context> options) : base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<UsersHostAccess>()
                .HasOne(c => (User)c.User)
                .WithMany(c=>c.UsersHostAccesses)
                .HasForeignKey(c=>c.UserId);

            modelBuilder
                .Entity<Partner>()
                .HasOne(c => (User)c.User);

            modelBuilder
                .Entity<UsersHostAccess>()
                .HasKey(c=> new { c.HostId, c.UserId });
        }
        public virtual DbSet<Host> Hosts { get; set; }
        public virtual DbSet<Segment> Segments { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Partner> Partners { get; set; }
        //public virtual DbSet<ProductTextTemplate> ProductTextTemplates { get; set; }
        //public virtual DbSet<PagePattern> PagePatterns { get; set; }
        //public virtual DbSet<UsersHostAccess> UsersHostAccess { get; set; }
        //public virtual DbSet<CategoryStatistic> CategoryStatistics { get; set; }
        //public virtual DbSet<ClickStatistic> ClickStatistics { get; set; }
        //public virtual DbSet<RequestResponseStatistic> RequestResponseStatistics { get; set; }
        //public virtual DbSet<SegmentTemplateStatistic> SegmentTemplateStatistics { get; set; }

    }
}
