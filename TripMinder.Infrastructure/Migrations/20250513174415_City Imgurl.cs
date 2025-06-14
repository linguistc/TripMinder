using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripMinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CityImgurl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Governorates",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Governorates");
        }
    }
}
