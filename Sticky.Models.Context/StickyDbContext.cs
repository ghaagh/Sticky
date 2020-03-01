using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Sticky.Models.Context
{
    public partial class StickyDbContext : DbContext
    {
        public StickyDbContext()
        {
        }

        public StickyDbContext(DbContextOptions<StickyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActionType> ActionTypes { get; set; }
        public virtual DbSet<ActivityType> ActivityTypes { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
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
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<ActionType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ActivityType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ActivityName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AudienceType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AudienceTypeName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<CategoryStat>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryStats)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryStats_RecordedCategories");
            });

            modelBuilder.Entity<Click>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.Clicks)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Clicks_Segments");
            });

            modelBuilder.Entity<Segment>(entity =>
            {
                entity.Property(e => e.ActionExtra).HasMaxLength(200);

                entity.Property(e => e.AudienceExtra).HasMaxLength(200);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreatorId).HasMaxLength(450);

                entity.Property(e => e.SegmentName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.Segments)
                    .HasForeignKey(d => d.ActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DruidSegments_ActionTypes");

                entity.HasOne(d => d.Audience)
                    .WithMany(p => p.Segments)
                    .HasForeignKey(d => d.AudienceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DruidSegments_AudienceTypes");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.DruidSegments)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK_DruidSegments_AspNetUsers");
            });


            modelBuilder.Entity<Host>(entity =>
            {
                entity.Property(e => e.AddToCardId).HasMaxLength(100);

                entity.Property(e => e.FinalizePage).HasMaxLength(100);

                entity.Property(e => e.HashCode).HasMaxLength(450);

                entity.Property(e => e.HostAddress).HasMaxLength(100);

                entity.Property(e => e.LogoAddress).HasMaxLength(2000);

                entity.Property(e => e.LogoOtherData).HasMaxLength(100);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.ValidatingHtmlAddress).HasMaxLength(400);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Hosts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Hosts_AspNetUsers");
            });


            modelBuilder.Entity<PartnerRequestLog>(entity =>
            {
                entity.Property(e => e.LogDate).HasColumnType("date");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.PartnerRequestLogs)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PartnerRequestLogs_Partners");
            });

            modelBuilder.Entity<Partner>(entity =>
            {
                entity.Property(e => e.CookieSyncAddress).HasMaxLength(400);

                entity.Property(e => e.Domain).HasMaxLength(50);

                entity.Property(e => e.ParnerHash)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.PartnerName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ProductTextTemplate>(entity =>
            {
                entity.Property(e => e.Template).IsRequired();

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.ProductTextTemplates)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductTextTemplates_DruidSegments");
            });

            modelBuilder.Entity<RecordedCategory>(entity =>
            {
                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Host)
                    .WithMany(p => p.RecordedCategories)
                    .HasForeignKey(d => d.HostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecordedCategories_Hosts");
            });

            modelBuilder.Entity<ResponseLogger>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");
            });
            modelBuilder.Entity<SegmentPagePattern>(entity =>
            {
                entity.Property(e => e.CreatorUserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.PagePattern)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.Property(e => e.PatternName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.CreatorUser)
                    .WithMany(p => p.SegmentPagePattern)
                    .HasForeignKey(d => d.CreatorUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentPagePattern_AspNetUsers");

                entity.HasOne(d => d.Host)
                    .WithMany(p => p.SegmentPagePattern)
                    .HasForeignKey(d => d.HostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentPagePattern_Hosts");
            });

            modelBuilder.Entity<SegmentStaticNative>(entity =>
            {
                entity.Property(e => e.NativeLogoAddress)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.NativeLogoOtherData).HasMaxLength(50);

                entity.Property(e => e.NativeText)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Segments)
                    .WithMany(p => p.SegmentStaticNatives)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentStaticNatives_Segments");
            });
            modelBuilder.Entity<Size>(entity =>
            {
                entity.Property(e => e.AdSize)
                    .IsRequired()
                    .HasMaxLength(100);
            });



            modelBuilder.Entity<UserTotalVisit>(entity =>
            {
                entity.Property(e => e.LogDate).HasColumnType("date");
            });

            modelBuilder.Entity<UsersHostAccess>(entity =>
            {
                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Host)
                    .WithMany(p => p.UsersHostAccess)
                    .HasForeignKey(d => d.HostId)
                    .HasConstraintName("FK_UsersHostAccess_Hosts");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersHostAccess)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UsersHostAccess_AspNetUsers");
            });
        }
    }
}
