using Microsoft.EntityFrameworkCore.Migrations;

namespace Sticky.Infrastructure.Migrations
{
    public partial class Initial5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActivityExtra",
                table: "Segments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityExtra",
                table: "Segments");
        }
    }
}
