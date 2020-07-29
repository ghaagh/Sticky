﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sticky.Models.Context;

namespace Sticky.API.Dashboard.Migrations
{
    [DbContext(typeof(StickyDbContext))]
    partial class StickyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = "1",
                            ConcurrencyStamp = "f0bb0816-220c-47e1-bf2a-5f428e5f1279",
                            Name = "ADMIN",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "2",
                            ConcurrencyStamp = "0281ac24-e499-4890-9ba5-2dc9fe550b4f",
                            Name = "HOSTOWNER",
                            NormalizedName = "HOSTOWNER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Sticky.Models.Context.ActionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ActionTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "فقط ذخیره کن"
                        },
                        new
                        {
                            Id = 2,
                            Name = "همان محصولات"
                        },
                        new
                        {
                            Id = 3,
                            Name = "کتگوری"
                        });
                });

            modelBuilder.Entity("Sticky.Models.Context.ActivityType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActivityName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ActivityTypes");
                });

            modelBuilder.Entity("Sticky.Models.Context.AudienceType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AudienceTypeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AudienceTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AudienceTypeName = "بازدید از صفحه"
                        },
                        new
                        {
                            Id = 2,
                            AudienceTypeName = "بازدید از محصول"
                        },
                        new
                        {
                            Id = 3,
                            AudienceTypeName = "افزودن به سبد"
                        },
                        new
                        {
                            Id = 4,
                            AudienceTypeName = "بازدید از کتگوری"
                        },
                        new
                        {
                            Id = 5,
                            AudienceTypeName = "خرید"
                        },
                        new
                        {
                            Id = 6,
                            AudienceTypeName = "افزودن به علاقه مندی ها"
                        });
                });

            modelBuilder.Entity("Sticky.Models.Context.CategoryStat", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("Counter")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("CategoryStats");
                });

            modelBuilder.Entity("Sticky.Models.Context.Click", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Count")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<int>("SegmentId")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SegmentId");

                    b.ToTable("Clicks");
                });

            modelBuilder.Entity("Sticky.Models.Context.Host", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddToCardId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("AddToCardValidated")
                        .HasColumnType("bit");

                    b.Property<bool>("CategoryValidated")
                        .HasColumnType("bit");

                    b.Property<bool?>("FavValidated")
                        .HasColumnType("bit");

                    b.Property<string>("FinalizePage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("FinalizeValidated")
                        .HasColumnType("bit");

                    b.Property<string>("HashCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HostAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("HostPriority")
                        .HasColumnType("bit");

                    b.Property<bool>("HostValidated")
                        .HasColumnType("bit");

                    b.Property<string>("LogoAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogoOtherData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PageValidated")
                        .HasColumnType("bit");

                    b.Property<int?>("ProductImageHeight")
                        .HasColumnType("int");

                    b.Property<int?>("ProductImageWidth")
                        .HasColumnType("int");

                    b.Property<bool>("ProductValidated")
                        .HasColumnType("bit");

                    b.Property<int?>("ProductValidityId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("UserValidityId")
                        .HasColumnType("int");

                    b.Property<string>("ValidatingHtmlAddress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Hosts");
                });

            modelBuilder.Entity("Sticky.Models.Context.Partner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CookieSyncAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Domain")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParnerHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartnerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Verified")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Partners");
                });

            modelBuilder.Entity("Sticky.Models.Context.PartnerRequestLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("DayCost")
                        .HasColumnType("float");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PartnerId")
                        .HasColumnType("int");

                    b.Property<int>("TotalRequestsCounter")
                        .HasColumnType("int");

                    b.Property<int>("TotalResponse")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PartnerId");

                    b.ToTable("PartnerRequestLogs");
                });

            modelBuilder.Entity("Sticky.Models.Context.ProductTextTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MaxPrice")
                        .HasColumnType("int");

                    b.Property<int?>("MinPrice")
                        .HasColumnType("int");

                    b.Property<int>("SegmentId")
                        .HasColumnType("int");

                    b.Property<string>("Template")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SegmentId");

                    b.ToTable("ProductTextTemplates");
                });

            modelBuilder.Entity("Sticky.Models.Context.RecordedCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Counter")
                        .HasColumnType("bigint");

                    b.Property<int>("HostId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HostId");

                    b.ToTable("RecordedCategories");
                });

            modelBuilder.Entity("Sticky.Models.Context.ResponseLogger", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Counter")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("PartnerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ResponseLogger");
                });

            modelBuilder.Entity("Sticky.Models.Context.Segment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActionExtra")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ActionId")
                        .HasColumnType("int");

                    b.Property<string>("AudienceExtra")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AudienceId")
                        .HasColumnType("int");

                    b.Property<long?>("AudienceNumber")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("HostId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<bool>("Paused")
                        .HasColumnType("bit");

                    b.Property<string>("SegmentName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ActionId");

                    b.HasIndex("AudienceId");

                    b.HasIndex("CreatorId");

                    b.HasIndex("HostId");

                    b.ToTable("Segments");
                });

            modelBuilder.Entity("Sticky.Models.Context.SegmentPagePattern", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatorUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<int>("HostId")
                        .HasColumnType("int");

                    b.Property<string>("PagePattern")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PatternName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorUserId");

                    b.HasIndex("HostId");

                    b.ToTable("SegmentPagePattern");
                });

            modelBuilder.Entity("Sticky.Models.Context.SegmentStat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Clicks")
                        .HasColumnType("int");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<int>("Impressions")
                        .HasColumnType("int");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<int>("SegmentId")
                        .HasColumnType("int");

                    b.Property<int?>("TextTemplateId")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("SegmentStats");
                });

            modelBuilder.Entity("Sticky.Models.Context.SegmentStaticNative", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("NativeLogoAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NativeLogoOtherData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NativeText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SegmentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SegmentId");

                    b.ToTable("SegmentStaticNatives");
                });

            modelBuilder.Entity("Sticky.Models.Context.Size", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdSize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("Sticky.Models.Context.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Sticky.Models.Context.UserTotalVisit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<int>("HostId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("UserTotalVisits");
                });

            modelBuilder.Entity("Sticky.Models.Context.UsersHostAccess", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AdminAccess")
                        .HasColumnType("bit");

                    b.Property<int?>("HostId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("HostId");

                    b.HasIndex("UserId");

                    b.ToTable("UsersHostAccess");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Sticky.Models.Context.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Sticky.Models.Context.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sticky.Models.Context.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Sticky.Models.Context.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Sticky.Models.Context.CategoryStat", b =>
                {
                    b.HasOne("Sticky.Models.Context.RecordedCategory", "Category")
                        .WithMany("CategoryStats")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Sticky.Models.Context.Click", b =>
                {
                    b.HasOne("Sticky.Models.Context.Segment", "Segment")
                        .WithMany("Clicks")
                        .HasForeignKey("SegmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Sticky.Models.Context.Host", b =>
                {
                    b.HasOne("Sticky.Models.Context.User", "User")
                        .WithMany("Hosts")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Sticky.Models.Context.PartnerRequestLog", b =>
                {
                    b.HasOne("Sticky.Models.Context.Partner", "Partner")
                        .WithMany("PartnerRequestLogs")
                        .HasForeignKey("PartnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Sticky.Models.Context.ProductTextTemplate", b =>
                {
                    b.HasOne("Sticky.Models.Context.Segment", "Segment")
                        .WithMany("ProductTextTemplates")
                        .HasForeignKey("SegmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Sticky.Models.Context.RecordedCategory", b =>
                {
                    b.HasOne("Sticky.Models.Context.Host", "Host")
                        .WithMany("RecordedCategories")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Sticky.Models.Context.Segment", b =>
                {
                    b.HasOne("Sticky.Models.Context.ActionType", "Action")
                        .WithMany("Segments")
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sticky.Models.Context.AudienceType", "Audience")
                        .WithMany("Segments")
                        .HasForeignKey("AudienceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sticky.Models.Context.User", "Creator")
                        .WithMany("Segments")
                        .HasForeignKey("CreatorId");

                    b.HasOne("Sticky.Models.Context.Host", "Host")
                        .WithMany("Segments")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Sticky.Models.Context.SegmentPagePattern", b =>
                {
                    b.HasOne("Sticky.Models.Context.User", "CreatorUser")
                        .WithMany()
                        .HasForeignKey("CreatorUserId");

                    b.HasOne("Sticky.Models.Context.Host", "Host")
                        .WithMany("SegmentPagePattern")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Sticky.Models.Context.SegmentStaticNative", b =>
                {
                    b.HasOne("Sticky.Models.Context.Segment", "Segments")
                        .WithMany("SegmentStaticNatives")
                        .HasForeignKey("SegmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Sticky.Models.Context.UsersHostAccess", b =>
                {
                    b.HasOne("Sticky.Models.Context.Host", "Host")
                        .WithMany("UsersHostAccess")
                        .HasForeignKey("HostId");

                    b.HasOne("Sticky.Models.Context.User", "User")
                        .WithMany("UsersHostAccesses")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
