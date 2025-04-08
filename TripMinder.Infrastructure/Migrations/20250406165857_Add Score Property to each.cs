using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripMinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddScorePropertytoeach : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Score",
                table: "TourismAreas",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Score",
                table: "Restaurants",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Score",
                table: "Entertainments",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "TourismAreas");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Entertainments");
        }
    }
}
