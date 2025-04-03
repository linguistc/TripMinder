using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripMinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accomodations_AccomodationsDescriptions_DescriptionId",
                table: "Accomodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Accomodations_Location_LocationId",
                table: "Accomodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Accomodations_PlaceCategories_CategoryID",
                table: "Accomodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Entertainments_EntertainmentsDescriptions_DescriptionId",
                table: "Entertainments");

            migrationBuilder.DropForeignKey(
                name: "FK_Entertainments_GeneralClasses_ClassId",
                table: "Entertainments");

            migrationBuilder.DropForeignKey(
                name: "FK_Entertainments_Location_LocationId",
                table: "Entertainments");

            migrationBuilder.DropForeignKey(
                name: "FK_Entertainments_PlaceCategories_CategoryID",
                table: "Entertainments");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_GeneralClasses_ClassId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Location_LocationId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_PlaceCategories_CategoryID",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_RestaurantDescriptions_DescriptionId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_TourismAreas_GeneralClasses_ClassId",
                table: "TourismAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_TourismAreas_Location_LocationId",
                table: "TourismAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_TourismAreas_PlaceCategories_CategoryID",
                table: "TourismAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_TourismAreas_TourismDescription_DescriptionId",
                table: "TourismAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Location_LocationId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AccomodationsDescriptions");

            migrationBuilder.DropTable(
                name: "AccomodationsImages");

            migrationBuilder.DropTable(
                name: "AccomodationsSocialProfiles");

            migrationBuilder.DropTable(
                name: "EntertainmentsDescriptions");

            migrationBuilder.DropTable(
                name: "EntertainmentsImages");

            migrationBuilder.DropTable(
                name: "EntertainmentsSocialProfiles");

            migrationBuilder.DropTable(
                name: "GeneralClasses");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "PlaceCategories");

            migrationBuilder.DropTable(
                name: "RestaurantDescriptions");

            migrationBuilder.DropTable(
                name: "RestaurantFoodCategories");

            migrationBuilder.DropTable(
                name: "RestaurantImages");

            migrationBuilder.DropTable(
                name: "RestaurantSocialProfiles");

            migrationBuilder.DropTable(
                name: "TourismDescription");

            migrationBuilder.DropTable(
                name: "TourismImages");

            migrationBuilder.DropTable(
                name: "TourismSocialProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Users_LocationId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TourismAreas_CategoryID",
                table: "TourismAreas");

            migrationBuilder.DropIndex(
                name: "IX_Restaurants_CategoryID",
                table: "Restaurants");

            migrationBuilder.DropIndex(
                name: "IX_Entertainments_CategoryID",
                table: "Entertainments");

            migrationBuilder.DropIndex(
                name: "IX_Accomodations_CategoryID",
                table: "Accomodations");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "TourismAreas");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Entertainments");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Accomodations");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "TourismAreas",
                newName: "TourismTypeId");

            migrationBuilder.RenameColumn(
                name: "DescriptionId",
                table: "TourismAreas",
                newName: "PlaceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_TourismAreas_LocationId",
                table: "TourismAreas",
                newName: "IX_TourismAreas_TourismTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_TourismAreas_DescriptionId",
                table: "TourismAreas",
                newName: "IX_TourismAreas_PlaceTypeId");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Restaurants",
                newName: "PlaceTypeId");

            migrationBuilder.RenameColumn(
                name: "DescriptionId",
                table: "Restaurants",
                newName: "FoodCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurants_LocationId",
                table: "Restaurants",
                newName: "IX_Restaurants_PlaceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurants_DescriptionId",
                table: "Restaurants",
                newName: "IX_Restaurants_FoodCategoryId");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Entertainments",
                newName: "PlaceTypeId");

            migrationBuilder.RenameColumn(
                name: "DescriptionId",
                table: "Entertainments",
                newName: "EntertainmentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Entertainments_LocationId",
                table: "Entertainments",
                newName: "IX_Entertainments_PlaceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Entertainments_DescriptionId",
                table: "Entertainments",
                newName: "IX_Entertainments_EntertainmentTypeId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AccomodationsClasses",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Accomodations",
                newName: "PlaceTypeId");

            migrationBuilder.RenameColumn(
                name: "DescriptionId",
                table: "Accomodations",
                newName: "AccomodationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Accomodations_LocationId",
                table: "Accomodations",
                newName: "IX_Accomodations_PlaceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Accomodations_DescriptionId",
                table: "Accomodations",
                newName: "IX_Accomodations_AccomodationTypeId");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "TourismAreas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactLink",
                table: "TourismAreas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TourismAreas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageSource",
                table: "TourismAreas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapLink",
                table: "TourismAreas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactLink",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageSource",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapLink",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Entertainments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactLink",
                table: "Entertainments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Entertainments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageSource",
                table: "Entertainments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapLink",
                table: "Entertainments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Accomodations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BedStatus",
                table: "Accomodations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactLink",
                table: "Accomodations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Accomodations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageSource",
                table: "Accomodations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapLink",
                table: "Accomodations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccomodationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccomodationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntertainmentsClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntertainmentsClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntertainmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntertainmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantsClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantsClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TourismAreaClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourismAreaClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TourismAreaTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourismAreaTypes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Accomodations_AccomodationTypes_AccomodationTypeId",
                table: "Accomodations",
                column: "AccomodationTypeId",
                principalTable: "AccomodationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Accomodations_PlaceTypes_PlaceTypeId",
                table: "Accomodations",
                column: "PlaceTypeId",
                principalTable: "PlaceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Entertainments_EntertainmentTypes_EntertainmentTypeId",
                table: "Entertainments",
                column: "EntertainmentTypeId",
                principalTable: "EntertainmentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Entertainments_EntertainmentsClasses_ClassId",
                table: "Entertainments",
                column: "ClassId",
                principalTable: "EntertainmentsClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Entertainments_PlaceTypes_PlaceTypeId",
                table: "Entertainments",
                column: "PlaceTypeId",
                principalTable: "PlaceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_FoodCategories_FoodCategoryId",
                table: "Restaurants",
                column: "FoodCategoryId",
                principalTable: "FoodCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_PlaceTypes_PlaceTypeId",
                table: "Restaurants",
                column: "PlaceTypeId",
                principalTable: "PlaceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_RestaurantsClasses_ClassId",
                table: "Restaurants",
                column: "ClassId",
                principalTable: "RestaurantsClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TourismAreas_PlaceTypes_PlaceTypeId",
                table: "TourismAreas",
                column: "PlaceTypeId",
                principalTable: "PlaceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TourismAreas_TourismAreaClasses_ClassId",
                table: "TourismAreas",
                column: "ClassId",
                principalTable: "TourismAreaClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TourismAreas_TourismAreaTypes_TourismTypeId",
                table: "TourismAreas",
                column: "TourismTypeId",
                principalTable: "TourismAreaTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accomodations_AccomodationTypes_AccomodationTypeId",
                table: "Accomodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Accomodations_PlaceTypes_PlaceTypeId",
                table: "Accomodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Entertainments_EntertainmentTypes_EntertainmentTypeId",
                table: "Entertainments");

            migrationBuilder.DropForeignKey(
                name: "FK_Entertainments_EntertainmentsClasses_ClassId",
                table: "Entertainments");

            migrationBuilder.DropForeignKey(
                name: "FK_Entertainments_PlaceTypes_PlaceTypeId",
                table: "Entertainments");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_FoodCategories_FoodCategoryId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_PlaceTypes_PlaceTypeId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_RestaurantsClasses_ClassId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_TourismAreas_PlaceTypes_PlaceTypeId",
                table: "TourismAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_TourismAreas_TourismAreaClasses_ClassId",
                table: "TourismAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_TourismAreas_TourismAreaTypes_TourismTypeId",
                table: "TourismAreas");

            migrationBuilder.DropTable(
                name: "AccomodationTypes");

            migrationBuilder.DropTable(
                name: "EntertainmentsClasses");

            migrationBuilder.DropTable(
                name: "EntertainmentTypes");

            migrationBuilder.DropTable(
                name: "PlaceTypes");

            migrationBuilder.DropTable(
                name: "RestaurantsClasses");

            migrationBuilder.DropTable(
                name: "TourismAreaClasses");

            migrationBuilder.DropTable(
                name: "TourismAreaTypes");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "TourismAreas");

            migrationBuilder.DropColumn(
                name: "ContactLink",
                table: "TourismAreas");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TourismAreas");

            migrationBuilder.DropColumn(
                name: "ImageSource",
                table: "TourismAreas");

            migrationBuilder.DropColumn(
                name: "MapLink",
                table: "TourismAreas");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ContactLink",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "ImageSource",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "MapLink",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Entertainments");

            migrationBuilder.DropColumn(
                name: "ContactLink",
                table: "Entertainments");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Entertainments");

            migrationBuilder.DropColumn(
                name: "ImageSource",
                table: "Entertainments");

            migrationBuilder.DropColumn(
                name: "MapLink",
                table: "Entertainments");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Accomodations");

            migrationBuilder.DropColumn(
                name: "BedStatus",
                table: "Accomodations");

            migrationBuilder.DropColumn(
                name: "ContactLink",
                table: "Accomodations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Accomodations");

            migrationBuilder.DropColumn(
                name: "ImageSource",
                table: "Accomodations");

            migrationBuilder.DropColumn(
                name: "MapLink",
                table: "Accomodations");

            migrationBuilder.RenameColumn(
                name: "TourismTypeId",
                table: "TourismAreas",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "PlaceTypeId",
                table: "TourismAreas",
                newName: "DescriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_TourismAreas_TourismTypeId",
                table: "TourismAreas",
                newName: "IX_TourismAreas_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_TourismAreas_PlaceTypeId",
                table: "TourismAreas",
                newName: "IX_TourismAreas_DescriptionId");

            migrationBuilder.RenameColumn(
                name: "PlaceTypeId",
                table: "Restaurants",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "FoodCategoryId",
                table: "Restaurants",
                newName: "DescriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurants_PlaceTypeId",
                table: "Restaurants",
                newName: "IX_Restaurants_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Restaurants_FoodCategoryId",
                table: "Restaurants",
                newName: "IX_Restaurants_DescriptionId");

            migrationBuilder.RenameColumn(
                name: "PlaceTypeId",
                table: "Entertainments",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "EntertainmentTypeId",
                table: "Entertainments",
                newName: "DescriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Entertainments_PlaceTypeId",
                table: "Entertainments",
                newName: "IX_Entertainments_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Entertainments_EntertainmentTypeId",
                table: "Entertainments",
                newName: "IX_Entertainments_DescriptionId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "AccomodationsClasses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "PlaceTypeId",
                table: "Accomodations",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "AccomodationTypeId",
                table: "Accomodations",
                newName: "DescriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Accomodations_PlaceTypeId",
                table: "Accomodations",
                newName: "IX_Accomodations_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Accomodations_AccomodationTypeId",
                table: "Accomodations",
                newName: "IX_Accomodations_DescriptionId");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "TourismAreas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Entertainments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Accomodations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccomodationsDescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccomodationsDescriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccomodationsImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccomodationId = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccomodationsImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccomodationsImages_Accomodations_AccomodationId",
                        column: x => x.AccomodationId,
                        principalTable: "Accomodations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AccomodationsSocialProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccomodationId = table.Column<int>(type: "int", nullable: false),
                    PlatformName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccomodationsSocialProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccomodationsSocialProfiles_Accomodations_AccomodationId",
                        column: x => x.AccomodationId,
                        principalTable: "Accomodations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EntertainmentsDescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntertainmentsDescriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntertainmentsImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntertainmentId = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntertainmentsImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntertainmentsImages_Entertainments_EntertainmentId",
                        column: x => x.EntertainmentId,
                        principalTable: "Entertainments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EntertainmentsSocialProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntertainmentId = table.Column<int>(type: "int", nullable: false),
                    PlatformName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntertainmentsSocialProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntertainmentsSocialProfiles_Entertainments_EntertainmentId",
                        column: x => x.EntertainmentId,
                        principalTable: "Entertainments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "GeneralClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaceCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantDescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantDescriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantFoodCategories",
                columns: table => new
                {
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    FoodCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantFoodCategories", x => new { x.RestaurantId, x.FoodCategoryId });
                    table.ForeignKey(
                        name: "FK_RestaurantFoodCategories_FoodCategories_FoodCategoryId",
                        column: x => x.FoodCategoryId,
                        principalTable: "FoodCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RestaurantFoodCategories_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantImages_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantSocialProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    PlatformName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantSocialProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantSocialProfiles_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TourismDescription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourismDescription", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TourismImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourismAreaId = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourismImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourismImages_TourismAreas_TourismAreaId",
                        column: x => x.TourismAreaId,
                        principalTable: "TourismAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TourismSocialProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourismAreaId = table.Column<int>(type: "int", nullable: false),
                    PlatformName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourismSocialProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourismSocialProfiles_TourismAreas_TourismAreaId",
                        column: x => x.TourismAreaId,
                        principalTable: "TourismAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_LocationId",
                table: "Users",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TourismAreas_CategoryID",
                table: "TourismAreas",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_CategoryID",
                table: "Restaurants",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Entertainments_CategoryID",
                table: "Entertainments",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Accomodations_CategoryID",
                table: "Accomodations",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_AccomodationsImages_AccomodationId",
                table: "AccomodationsImages",
                column: "AccomodationId");

            migrationBuilder.CreateIndex(
                name: "IX_AccomodationsSocialProfiles_AccomodationId",
                table: "AccomodationsSocialProfiles",
                column: "AccomodationId");

            migrationBuilder.CreateIndex(
                name: "IX_EntertainmentsImages_EntertainmentId",
                table: "EntertainmentsImages",
                column: "EntertainmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntertainmentsSocialProfiles_EntertainmentId",
                table: "EntertainmentsSocialProfiles",
                column: "EntertainmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantFoodCategories_FoodCategoryId",
                table: "RestaurantFoodCategories",
                column: "FoodCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantImages_RestaurantId",
                table: "RestaurantImages",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantSocialProfiles_RestaurantId",
                table: "RestaurantSocialProfiles",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_TourismImages_TourismAreaId",
                table: "TourismImages",
                column: "TourismAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_TourismSocialProfiles_TourismAreaId",
                table: "TourismSocialProfiles",
                column: "TourismAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accomodations_AccomodationsDescriptions_DescriptionId",
                table: "Accomodations",
                column: "DescriptionId",
                principalTable: "AccomodationsDescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Accomodations_Location_LocationId",
                table: "Accomodations",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Accomodations_PlaceCategories_CategoryID",
                table: "Accomodations",
                column: "CategoryID",
                principalTable: "PlaceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Entertainments_EntertainmentsDescriptions_DescriptionId",
                table: "Entertainments",
                column: "DescriptionId",
                principalTable: "EntertainmentsDescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Entertainments_GeneralClasses_ClassId",
                table: "Entertainments",
                column: "ClassId",
                principalTable: "GeneralClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Entertainments_Location_LocationId",
                table: "Entertainments",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Entertainments_PlaceCategories_CategoryID",
                table: "Entertainments",
                column: "CategoryID",
                principalTable: "PlaceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_GeneralClasses_ClassId",
                table: "Restaurants",
                column: "ClassId",
                principalTable: "GeneralClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Location_LocationId",
                table: "Restaurants",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_PlaceCategories_CategoryID",
                table: "Restaurants",
                column: "CategoryID",
                principalTable: "PlaceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_RestaurantDescriptions_DescriptionId",
                table: "Restaurants",
                column: "DescriptionId",
                principalTable: "RestaurantDescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TourismAreas_GeneralClasses_ClassId",
                table: "TourismAreas",
                column: "ClassId",
                principalTable: "GeneralClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TourismAreas_Location_LocationId",
                table: "TourismAreas",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TourismAreas_PlaceCategories_CategoryID",
                table: "TourismAreas",
                column: "CategoryID",
                principalTable: "PlaceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TourismAreas_TourismDescription_DescriptionId",
                table: "TourismAreas",
                column: "DescriptionId",
                principalTable: "TourismDescription",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Location_LocationId",
                table: "Users",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
