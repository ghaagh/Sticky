using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sticky.Infrastructure.Migrations
{
    public partial class Initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Segment_Hosts_HostId",
                table: "Segment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Segment",
                table: "Segment");

            migrationBuilder.RenameTable(
                name: "Segment",
                newName: "Segments");

            migrationBuilder.RenameIndex(
                name: "IX_Segment_HostId",
                table: "Segments",
                newName: "IX_Segments_HostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Segments",
                table: "Segments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CookieSyncAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Verified = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partners_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestResponseStatistic",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartnerId = table.Column<int>(type: "int", nullable: false),
                    Response = table.Column<int>(type: "int", nullable: false),
                    DayCost = table.Column<double>(type: "float", nullable: false),
                    PartnerId1 = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestResponseStatistic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestResponseStatistic_Partners_PartnerId1",
                        column: x => x.PartnerId1,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Partners_UserId",
                table: "Partners",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestResponseStatistic_PartnerId1",
                table: "RequestResponseStatistic",
                column: "PartnerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Segments_Hosts_HostId",
                table: "Segments",
                column: "HostId",
                principalTable: "Hosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Segments_Hosts_HostId",
                table: "Segments");

            migrationBuilder.DropTable(
                name: "RequestResponseStatistic");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Segments",
                table: "Segments");

            migrationBuilder.RenameTable(
                name: "Segments",
                newName: "Segment");

            migrationBuilder.RenameIndex(
                name: "IX_Segments_HostId",
                table: "Segment",
                newName: "IX_Segment_HostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Segment",
                table: "Segment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Segment_Hosts_HostId",
                table: "Segment",
                column: "HostId",
                principalTable: "Hosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
