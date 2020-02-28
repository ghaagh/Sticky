using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
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

        public virtual DbSet<ActionTypes> ActionTypes { get; set; }
        public virtual DbSet<ActivityTypes> ActivityTypes { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AudienceTypes> AudienceTypes { get; set; }
        public virtual DbSet<CategoryStats> CategoryStats { get; set; }
        public virtual DbSet<Clicks> Clicks { get; set; }
        public virtual DbSet<CreatedSegments> CreatedSegments { get; set; }
        public virtual DbSet<SegmentOwnership> DruidSegmentOwnership { get; set; }
        public virtual DbSet<Segments> DruidSegments { get; set; }
        public virtual DbSet<DynamicAdHtml> DynamicAdHtml { get; set; }
        public virtual DbSet<SegmentTextTemplates> DynamicSegmentTextTemplates { get; set; }
        public virtual DbSet<Filters> Filters { get; set; }
        public virtual DbSet<FiltersForSegments> FiltersForSegments { get; set; }
        public virtual DbSet<GroupedProductDetails> GroupedProductDetails { get; set; }
        public virtual DbSet<Hosts> Hosts { get; set; }
        public virtual DbSet<PartnerRequestLogs> PartnerRequestLogs { get; set; }
        public virtual DbSet<Partners> Partners { get; set; }
        public virtual DbSet<ProductGroupingTypes> ProductGroupingTypes { get; set; }
        public virtual DbSet<ProductGroupings> ProductGroupings { get; set; }
        public virtual DbSet<ProductTextTemplates> ProductTextTemplates { get; set; }
        public virtual DbSet<RecordedCategories> RecordedCategories { get; set; }
        public virtual DbSet<ResponseLogger> ResponseLogger { get; set; }
        public virtual DbSet<SegmentPagePattern> SegmentPagePattern { get; set; }
        public virtual DbSet<SegmentStaticNatives> SegmentStaticNatives { get; set; }
        public virtual DbSet<SegmentStats> SegmentStats { get; set; }
        public virtual DbSet<Sizes> Sizes { get; set; }
        public virtual DbSet<TblSegmentsResultTypes> TblSegmentsResultTypes { get; set; }
        public virtual DbSet<TrackedUsers> TrackedUsers { get; set; }
        public virtual DbSet<UserTotalVisits> UserTotalVisits { get; set; }
        public virtual DbSet<UsersHostAccess> UsersHostAccess { get; set; }
        public virtual DbSet<ValidityTimeTypes> ValidityTimeTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<ActionTypes>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ActivityTypes>(entity =>
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

            modelBuilder.Entity<AudienceTypes>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AudienceTypeName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<CategoryStats>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryStats)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryStats_RecordedCategories");
            });

            modelBuilder.Entity<Clicks>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.Clicks)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Clicks_CreatedSegments");
            });

            modelBuilder.Entity<CreatedSegments>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreatorId).HasMaxLength(450);

                entity.Property(e => e.DruidQuery).HasMaxLength(300);

                entity.Property(e => e.DruidSubFilter).HasMaxLength(300);

                entity.Property(e => e.ExtraQuery).HasMaxLength(450);

                entity.Property(e => e.ResultValue).HasMaxLength(1000);

                entity.Property(e => e.SegmentName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.CreatedSegments)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK_CreatedSegments_AspNetUsers");

                entity.HasOne(d => d.Host)
                    .WithMany(p => p.CreatedSegments)
                    .HasForeignKey(d => d.HostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CreatedSegments_Hosts");

                entity.HasOne(d => d.ResultType)
                    .WithMany(p => p.CreatedSegments)
                    .HasForeignKey(d => d.ResultTypeId)
                    .HasConstraintName("FK_CreatedSegments_tblSegmentsResultTypes");
            });

            modelBuilder.Entity<SegmentOwnership>(entity =>
            {
                entity.Property(e => e.SubFilter).HasMaxLength(200);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Host)
                    .WithMany(p => p.DruidSegmentOwnership)
                    .HasForeignKey(d => d.HostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DruidSegmentOwnership_Hosts");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.DruidSegmentOwnership)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DruidSegmentOwnership_CreatedSegments");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DruidSegmentOwnership)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DruidSegmentOwnership_AspNetUsers");
            });

            modelBuilder.Entity<Segments>(entity =>
            {
                entity.Property(e => e.ActionExtra).HasMaxLength(200);

                entity.Property(e => e.AudienceExtra).HasMaxLength(200);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreatorId).HasMaxLength(450);

                entity.Property(e => e.SegmentName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.DruidSegments)
                    .HasForeignKey(d => d.ActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DruidSegments_ActionTypes");

                entity.HasOne(d => d.Audience)
                    .WithMany(p => p.DruidSegments)
                    .HasForeignKey(d => d.AudienceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DruidSegments_AudienceTypes");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.DruidSegments)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("FK_DruidSegments_AspNetUsers");
            });

            modelBuilder.Entity<DynamicAdHtml>(entity =>
            {
                entity.Property(e => e.Html)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Host)
                    .WithMany(p => p.DynamicAdHtml)
                    .HasForeignKey(d => d.HostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DynamicAdHtml_Hosts");

                entity.HasOne(d => d.Size)
                    .WithMany(p => p.DynamicAdHtml)
                    .HasForeignKey(d => d.SizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DynamicAdHtml_Sizes");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DynamicAdHtml)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DynamicAdHtml_AspNetUsers");
            });

            modelBuilder.Entity<SegmentTextTemplates>(entity =>
            {
                entity.Property(e => e.Template).IsRequired();

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.DynamicSegmentTextTemplates)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DynamicSegmentTextTemplates_CreatedSegments");
            });

            modelBuilder.Entity<Filters>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TitleFa)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<FiltersForSegments>(entity =>
            {
                entity.Property(e => e.FilterValue)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.HasOne(d => d.Filter)
                    .WithMany(p => p.FiltersForSegments)
                    .HasForeignKey(d => d.FilterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FiltersForSegments_Filters");

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.FiltersForSegments)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FiltersForSegments_CreatedSegments");
            });

            modelBuilder.Entity<GroupedProductDetails>(entity =>
            {
                entity.Property(e => e.Value).HasMaxLength(100);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupedProductDetails)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_GroupedProductDetails_ProductGroupings");

                entity.HasOne(d => d.GroupType)
                    .WithMany(p => p.GroupedProductDetails)
                    .HasForeignKey(d => d.GroupTypeId)
                    .HasConstraintName("FK_GroupedProductDetails_ProductGroupingTypes");
            });

            modelBuilder.Entity<Hosts>(entity =>
            {
                entity.Property(e => e.AddToCardId).HasMaxLength(100);

                entity.Property(e => e.FinalizePage).HasMaxLength(100);

                entity.Property(e => e.HashCode).HasMaxLength(450);

                entity.Property(e => e.Host).HasMaxLength(100);

                entity.Property(e => e.LogoAddress).HasMaxLength(2000);

                entity.Property(e => e.LogoOtherData).HasMaxLength(100);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.ValidatingHtmlAddress).HasMaxLength(400);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Hosts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Hosts_AspNetUsers");
            });


            modelBuilder.Entity<PartnerRequestLogs>(entity =>
            {
                entity.Property(e => e.LogDate).HasColumnType("date");

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.PartnerRequestLogs)
                    .HasForeignKey(d => d.PartnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PartnerRequestLogs_Partners");
            });

            modelBuilder.Entity<Partners>(entity =>
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

            modelBuilder.Entity<ProductGroupingTypes>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.GroupingTypeName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.GroupingTypeNameFa).HasMaxLength(100);
            });

            modelBuilder.Entity<ProductGroupings>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProductGroupings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ProductGroupings_AspNetUsers");
            });

            modelBuilder.Entity<ProductTextTemplates>(entity =>
            {
                entity.Property(e => e.Template).IsRequired();

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.ProductTextTemplates)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductTextTemplates_DruidSegments");
            });

            modelBuilder.Entity<RecordedCategories>(entity =>
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

            modelBuilder.Entity<RemoveListAfterMembership>(entity =>
            {
                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.RemoveListAfterMembership)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RemoveListAfterMembership_CreatedSegments");
            });

            modelBuilder.Entity<ResponseLogger>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");
            });

            modelBuilder.Entity<SegmentImages>(entity =>
            {
                entity.HasKey(e => new { e.SegmentId, e.SizeId });

                entity.Property(e => e.ImageAddress)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Other).HasMaxLength(100);

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.SegmentImages)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentImages_CreatedSegments");

                entity.HasOne(d => d.Size)
                    .WithMany(p => p.SegmentImages)
                    .HasForeignKey(d => d.SizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentImages_Sizes");
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

            modelBuilder.Entity<SegmentStaticNatives>(entity =>
            {
                entity.Property(e => e.NativeLogoAddress)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.NativeLogoOtherData).HasMaxLength(50);

                entity.Property(e => e.NativeText)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.SegmentStaticNatives)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentStaticNatives_CreatedSegments");
            });

            modelBuilder.Entity<SegmentTags>(entity =>
            {
                entity.HasKey(e => new { e.SegmentId, e.SizeId });

                entity.Property(e => e.Tag).IsRequired();

                entity.HasOne(d => d.Segment)
                    .WithMany(p => p.SegmentTags)
                    .HasForeignKey(d => d.SegmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentTags_CreatedSegments");

                entity.HasOne(d => d.Size)
                    .WithMany(p => p.SegmentTags)
                    .HasForeignKey(d => d.SizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SegmentTags_Sizes");
            });

            modelBuilder.Entity<Sizes>(entity =>
            {
                entity.Property(e => e.AdSize)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<TblSegmentsResultTypes>(entity =>
            {
                entity.ToTable("tblSegmentsResultTypes");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ResultTypeName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ResultTypeNameFa)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<TrackedUsers>(entity =>
            {
                entity.Property(e => e.LogDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<UserTotalVisits>(entity =>
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

            modelBuilder.Entity<ValidityTimeTypes>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NameFa)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}
