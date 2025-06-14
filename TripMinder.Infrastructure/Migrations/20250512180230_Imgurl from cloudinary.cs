using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripMinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Imgurlfromcloudinary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgData",
                table: "TourismAreas");

            migrationBuilder.DropColumn(
                name: "ImgData",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ImgData",
                table: "Entertainments");

            migrationBuilder.DropColumn(
                name: "ImgData",
                table: "Accomodations");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "TourismAreas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Entertainments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Accomodations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "TourismAreas");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Entertainments");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Accomodations");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImgData",
                table: "TourismAreas",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImgData",
                table: "Restaurants",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImgData",
                table: "Entertainments",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImgData",
                table: "Accomodations",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
