using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripMinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImgBLOB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageSource",
                table: "TourismAreas");

            migrationBuilder.DropColumn(
                name: "ImageSource",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ImageSource",
                table: "Entertainments");

            migrationBuilder.DropColumn(
                name: "ImageSource",
                table: "Accomodations");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImgData",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgData",
                table: "Users");

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
                name: "ImageSource",
                table: "TourismAreas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageSource",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageSource",
                table: "Entertainments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageSource",
                table: "Accomodations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
