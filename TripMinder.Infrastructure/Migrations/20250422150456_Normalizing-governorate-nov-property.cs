using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripMinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Normalizinggovernoratenovproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accomodations_Governorates_GovernorateId",
                table: "Accomodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Entertainments_Governorates_GovernorateId",
                table: "Entertainments");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Governorates_GovernorateId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_TourismAreas_Governorates_GovernorateId",
                table: "TourismAreas");

            migrationBuilder.DropIndex(
                name: "IX_TourismAreas_GovernorateId",
                table: "TourismAreas");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_GovernorateId",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Entertainments_GovernorateId",
                table: "Entertainments");

            migrationBuilder.DropIndex(
                name: "IX_Accomodations_GovernorateId",
                table: "Accomodations");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "TourismAreas");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "Entertainments");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "Accomodations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "TourismAreas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "Entertainments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "Accomodations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TourismAreas_GovernorateId",
                table: "TourismAreas",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_GovernorateId",
                table: "Restaurants",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_Entertainments_GovernorateId",
                table: "Entertainments",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_Accomodations_GovernorateId",
                table: "Accomodations",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accomodations_Governorates_GovernorateId",
                table: "Accomodations",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entertainments_Governorates_GovernorateId",
                table: "Entertainments",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Governorates_GovernorateId",
                table: "Restaurants",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TourismAreas_Governorates_GovernorateId",
                table: "TourismAreas",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
