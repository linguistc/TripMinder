using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripMinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Place_Class_ClassId",
                table: "Place");

            migrationBuilder.DropForeignKey(
                name: "FK_Place_Description_DescriptionId",
                table: "Place");

            migrationBuilder.DropForeignKey(
                name: "FK_Place_PlaceCategories_CategoryID",
                table: "Place");

            migrationBuilder.DropForeignKey(
                name: "FK_Place_Zones_ZoneId",
                table: "Place");

            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantFoodCategories_Place_RestaurantId",
                table: "RestaurantFoodCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersBookMarks_Place_PlaceId",
                table: "UsersBookMarks");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersRatings_Place_PlaceId",
                table: "UsersRatings");

            migrationBuilder.DropTable(
                name: "BusinessSocialProfiles");

            migrationBuilder.DropTable(
                name: "PlaceImages");

            migrationBuilder.DropIndex(
                name: "IX_UsersRatings_PlaceId",
                table: "UsersRatings");

            migrationBuilder.DropIndex(
                name: "IX_UsersBookMarks_PlaceId",
                table: "UsersBookMarks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Place",
                table: "Place");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Description",
                table: "Description");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Class",
                table: "Class");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "UsersRatings");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "UsersBookMarks");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Place");

            migrationBuilder.DropColumn(
                name: "Location_Address",
                table: "Place");

            migrationBuilder.DropColumn(
                name: "Location_Latitude",
                table: "Place");

            migrationBuilder.DropColumn(
                name: "Location_Longitude",
                table: "Place");

            migrationBuilder.DropColumn(
                name: "NumOfBeds",
                table: "Place");

            migrationBuilder.DropColumn(
                name: "SocialMediaLink",
                table: "Place");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Description");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Class");

            migrationBuilder.RenameTable(
                name: "Place",
                newName: "TourismAreas");

            migrationBuilder.RenameTable(
                name: "Description",
                newName: "RestaurantDescriptions");

            migrationBuilder.RenameTable(
                name: "Class",
                newName: "GeneralClasses");

            migrationBuilder.RenameColumn(
                name: "NumOfMembers",
                table: "TourismAreas",
                newName: "TripSuggestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Place_ZoneId",
                table: "TourismAreas",
                newName: "IX_TourismAreas_ZoneId");

            migrationBuilder.RenameIndex(
                name: "IX_Place_DescriptionId",
                table: "TourismAreas",
                newName: "IX_TourismAreas_DescriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Place_ClassId",
                table: "TourismAreas",
                newName: "IX_TourismAreas_ClassId");

            migrationBuilder.RenameIndex(
                name: "IX_Place_CategoryID",
                table: "TourismAreas",
                newName: "IX_TourismAreas_CategoryID");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "TourismAreas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TourismAreas",
                table: "TourismAreas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RestaurantDescriptions",
                table: "RestaurantDescriptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeneralClasses",
                table: "GeneralClasses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AccomodationsClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccomodationsClasses", x => x.Id);
                });

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
                name: "BookMarkTourisms",
                columns: table => new
                {
                    BookmarkId = table.Column<int>(type: "int", nullable: false),
                    TourismId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookMarkTourisms", x => new { x.BookmarkId, x.TourismId });
                    table.ForeignKey(
                        name: "FK_BookMarkTourisms_TourismAreas_TourismId",
                        column: x => x.TourismId,
                        principalTable: "TourismAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BookMarkTourisms_UsersBookMarks_BookmarkId",
                        column: x => x.BookmarkId,
                        principalTable: "UsersBookMarks",
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
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
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
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TourismAreaId = table.Column<int>(type: "int", nullable: false)
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
                    PlatformName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TourismAreaId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "TripSuggestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Budget = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripSuggestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accomodations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
                    AveragePricePerAdult = table.Column<double>(type: "float", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    HasKidsArea = table.Column<bool>(type: "bit", nullable: false),
                    NumOfBeds = table.Column<int>(type: "int", nullable: false),
                    NumOfMembers = table.Column<int>(type: "int", nullable: false),
                    TripSuggestionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accomodations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accomodations_AccomodationsClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "AccomodationsClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Accomodations_AccomodationsDescriptions_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "AccomodationsDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Accomodations_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Accomodations_PlaceCategories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "PlaceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Accomodations_TripSuggestions_TripSuggestionId",
                        column: x => x.TripSuggestionId,
                        principalTable: "TripSuggestions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Accomodations_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BookMarkTrips",
                columns: table => new
                {
                    BookmarkId = table.Column<int>(type: "int", nullable: false),
                    TripSuggestionId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookMarkTrips", x => new { x.BookmarkId, x.TripSuggestionId });
                    table.ForeignKey(
                        name: "FK_BookMarkTrips_TripSuggestions_TripSuggestionId",
                        column: x => x.TripSuggestionId,
                        principalTable: "TripSuggestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BookMarkTrips_UsersBookMarks_BookmarkId",
                        column: x => x.BookmarkId,
                        principalTable: "UsersBookMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Entertainments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
                    AveragePricePerAdult = table.Column<double>(type: "float", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    HasKidsArea = table.Column<bool>(type: "bit", nullable: false),
                    TripSuggestionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entertainments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entertainments_EntertainmentsDescriptions_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "EntertainmentsDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Entertainments_GeneralClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "GeneralClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Entertainments_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Entertainments_PlaceCategories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "PlaceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Entertainments_TripSuggestions_TripSuggestionId",
                        column: x => x.TripSuggestionId,
                        principalTable: "TripSuggestions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entertainments_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
                    AveragePricePerAdult = table.Column<double>(type: "float", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    HasKidsArea = table.Column<bool>(type: "bit", nullable: false),
                    TripSuggestionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restaurants_GeneralClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "GeneralClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Restaurants_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Restaurants_PlaceCategories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "PlaceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Restaurants_RestaurantDescriptions_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "RestaurantDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Restaurants_TripSuggestions_TripSuggestionId",
                        column: x => x.TripSuggestionId,
                        principalTable: "TripSuggestions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Restaurants_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TourismSuggestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuggestionId = table.Column<int>(type: "int", nullable: false),
                    TourismAreaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourismSuggestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourismSuggestions_TourismAreas_TourismAreaId",
                        column: x => x.TourismAreaId,
                        principalTable: "TourismAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TourismSuggestions_TripSuggestions_SuggestionId",
                        column: x => x.SuggestionId,
                        principalTable: "TripSuggestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AccomodationsImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccomodationId = table.Column<int>(type: "int", nullable: false)
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
                    PlatformName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccomodationId = table.Column<int>(type: "int", nullable: false)
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
                name: "AccomodationsSuggestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuggestionId = table.Column<int>(type: "int", nullable: false),
                    AccomodationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccomodationsSuggestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccomodationsSuggestions_Accomodations_AccomodationId",
                        column: x => x.AccomodationId,
                        principalTable: "Accomodations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AccomodationsSuggestions_TripSuggestions_SuggestionId",
                        column: x => x.SuggestionId,
                        principalTable: "TripSuggestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BookMarkAccomodations",
                columns: table => new
                {
                    BookmarkId = table.Column<int>(type: "int", nullable: false),
                    AccomodationId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookMarkAccomodations", x => new { x.BookmarkId, x.AccomodationId });
                    table.ForeignKey(
                        name: "FK_BookMarkAccomodations_Accomodations_AccomodationId",
                        column: x => x.AccomodationId,
                        principalTable: "Accomodations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BookMarkAccomodations_UsersBookMarks_BookmarkId",
                        column: x => x.BookmarkId,
                        principalTable: "UsersBookMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BookMarkEntertainments",
                columns: table => new
                {
                    BookmarkId = table.Column<int>(type: "int", nullable: false),
                    EntertainmentId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookMarkEntertainments", x => new { x.BookmarkId, x.EntertainmentId });
                    table.ForeignKey(
                        name: "FK_BookMarkEntertainments_Entertainments_EntertainmentId",
                        column: x => x.EntertainmentId,
                        principalTable: "Entertainments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BookMarkEntertainments_UsersBookMarks_BookmarkId",
                        column: x => x.BookmarkId,
                        principalTable: "UsersBookMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EntertainmentsImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntertainmentId = table.Column<int>(type: "int", nullable: false)
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
                    PlatformName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntertainmentId = table.Column<int>(type: "int", nullable: false)
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
                name: "EntertainmentsSuggestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuggestionId = table.Column<int>(type: "int", nullable: false),
                    EntertainmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntertainmentsSuggestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntertainmentsSuggestions_Entertainments_EntertainmentId",
                        column: x => x.EntertainmentId,
                        principalTable: "Entertainments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EntertainmentsSuggestions_TripSuggestions_SuggestionId",
                        column: x => x.SuggestionId,
                        principalTable: "TripSuggestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BookMarkRestaurants",
                columns: table => new
                {
                    BookmarkId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookMarkRestaurants", x => new { x.BookmarkId, x.RestaurantId });
                    table.ForeignKey(
                        name: "FK_BookMarkRestaurants_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BookMarkRestaurants_UsersBookMarks_BookmarkId",
                        column: x => x.BookmarkId,
                        principalTable: "UsersBookMarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false)
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
                    PlatformName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false)
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
                name: "RestaurantSuggestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuggestionId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantSuggestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantSuggestions_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RestaurantSuggestions_TripSuggestions_SuggestionId",
                        column: x => x.SuggestionId,
                        principalTable: "TripSuggestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_LocationId",
                table: "Users",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TourismAreas_LocationId",
                table: "TourismAreas",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TourismAreas_TripSuggestionId",
                table: "TourismAreas",
                column: "TripSuggestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Accomodations_CategoryID",
                table: "Accomodations",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Accomodations_ClassId",
                table: "Accomodations",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Accomodations_DescriptionId",
                table: "Accomodations",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Accomodations_LocationId",
                table: "Accomodations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Accomodations_TripSuggestionId",
                table: "Accomodations",
                column: "TripSuggestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Accomodations_ZoneId",
                table: "Accomodations",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_AccomodationsImages_AccomodationId",
                table: "AccomodationsImages",
                column: "AccomodationId");

            migrationBuilder.CreateIndex(
                name: "IX_AccomodationsSocialProfiles_AccomodationId",
                table: "AccomodationsSocialProfiles",
                column: "AccomodationId");

            migrationBuilder.CreateIndex(
                name: "IX_AccomodationsSuggestions_AccomodationId",
                table: "AccomodationsSuggestions",
                column: "AccomodationId");

            migrationBuilder.CreateIndex(
                name: "IX_AccomodationsSuggestions_SuggestionId",
                table: "AccomodationsSuggestions",
                column: "SuggestionId");

            migrationBuilder.CreateIndex(
                name: "IX_BookMarkAccomodations_AccomodationId",
                table: "BookMarkAccomodations",
                column: "AccomodationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookMarkEntertainments_EntertainmentId",
                table: "BookMarkEntertainments",
                column: "EntertainmentId");

            migrationBuilder.CreateIndex(
                name: "IX_BookMarkRestaurants_RestaurantId",
                table: "BookMarkRestaurants",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_BookMarkTourisms_TourismId",
                table: "BookMarkTourisms",
                column: "TourismId");

            migrationBuilder.CreateIndex(
                name: "IX_BookMarkTrips_TripSuggestionId",
                table: "BookMarkTrips",
                column: "TripSuggestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Entertainments_CategoryID",
                table: "Entertainments",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Entertainments_ClassId",
                table: "Entertainments",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Entertainments_DescriptionId",
                table: "Entertainments",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Entertainments_LocationId",
                table: "Entertainments",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Entertainments_TripSuggestionId",
                table: "Entertainments",
                column: "TripSuggestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Entertainments_ZoneId",
                table: "Entertainments",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_EntertainmentsImages_EntertainmentId",
                table: "EntertainmentsImages",
                column: "EntertainmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntertainmentsSocialProfiles_EntertainmentId",
                table: "EntertainmentsSocialProfiles",
                column: "EntertainmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntertainmentsSuggestions_EntertainmentId",
                table: "EntertainmentsSuggestions",
                column: "EntertainmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntertainmentsSuggestions_SuggestionId",
                table: "EntertainmentsSuggestions",
                column: "SuggestionId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantImages_RestaurantId",
                table: "RestaurantImages",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_CategoryID",
                table: "Restaurants",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_ClassId",
                table: "Restaurants",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_DescriptionId",
                table: "Restaurants",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_LocationId",
                table: "Restaurants",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_TripSuggestionId",
                table: "Restaurants",
                column: "TripSuggestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_ZoneId",
                table: "Restaurants",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantSocialProfiles_RestaurantId",
                table: "RestaurantSocialProfiles",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantSuggestions_RestaurantId",
                table: "RestaurantSuggestions",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantSuggestions_SuggestionId",
                table: "RestaurantSuggestions",
                column: "SuggestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TourismImages_TourismAreaId",
                table: "TourismImages",
                column: "TourismAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_TourismSocialProfiles_TourismAreaId",
                table: "TourismSocialProfiles",
                column: "TourismAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_TourismSuggestions_SuggestionId",
                table: "TourismSuggestions",
                column: "SuggestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TourismSuggestions_TourismAreaId",
                table: "TourismSuggestions",
                column: "TourismAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantFoodCategories_Restaurants_RestaurantId",
                table: "RestaurantFoodCategories",
                column: "RestaurantId",
                principalTable: "Restaurants",
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
                name: "FK_TourismAreas_TripSuggestions_TripSuggestionId",
                table: "TourismAreas",
                column: "TripSuggestionId",
                principalTable: "TripSuggestions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TourismAreas_Zones_ZoneId",
                table: "TourismAreas",
                column: "ZoneId",
                principalTable: "Zones",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantFoodCategories_Restaurants_RestaurantId",
                table: "RestaurantFoodCategories");

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
                name: "FK_TourismAreas_TripSuggestions_TripSuggestionId",
                table: "TourismAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_TourismAreas_Zones_ZoneId",
                table: "TourismAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Location_LocationId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AccomodationsImages");

            migrationBuilder.DropTable(
                name: "AccomodationsSocialProfiles");

            migrationBuilder.DropTable(
                name: "AccomodationsSuggestions");

            migrationBuilder.DropTable(
                name: "BookMarkAccomodations");

            migrationBuilder.DropTable(
                name: "BookMarkEntertainments");

            migrationBuilder.DropTable(
                name: "BookMarkRestaurants");

            migrationBuilder.DropTable(
                name: "BookMarkTourisms");

            migrationBuilder.DropTable(
                name: "BookMarkTrips");

            migrationBuilder.DropTable(
                name: "EntertainmentsImages");

            migrationBuilder.DropTable(
                name: "EntertainmentsSocialProfiles");

            migrationBuilder.DropTable(
                name: "EntertainmentsSuggestions");

            migrationBuilder.DropTable(
                name: "RestaurantImages");

            migrationBuilder.DropTable(
                name: "RestaurantSocialProfiles");

            migrationBuilder.DropTable(
                name: "RestaurantSuggestions");

            migrationBuilder.DropTable(
                name: "TourismDescription");

            migrationBuilder.DropTable(
                name: "TourismImages");

            migrationBuilder.DropTable(
                name: "TourismSocialProfiles");

            migrationBuilder.DropTable(
                name: "TourismSuggestions");

            migrationBuilder.DropTable(
                name: "Accomodations");

            migrationBuilder.DropTable(
                name: "Entertainments");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropTable(
                name: "AccomodationsClasses");

            migrationBuilder.DropTable(
                name: "AccomodationsDescriptions");

            migrationBuilder.DropTable(
                name: "EntertainmentsDescriptions");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "TripSuggestions");

            migrationBuilder.DropIndex(
                name: "IX_Users_LocationId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TourismAreas",
                table: "TourismAreas");

            migrationBuilder.DropIndex(
                name: "IX_TourismAreas_LocationId",
                table: "TourismAreas");

            migrationBuilder.DropIndex(
                name: "IX_TourismAreas_TripSuggestionId",
                table: "TourismAreas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RestaurantDescriptions",
                table: "RestaurantDescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeneralClasses",
                table: "GeneralClasses");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "TourismAreas");

            migrationBuilder.RenameTable(
                name: "TourismAreas",
                newName: "Place");

            migrationBuilder.RenameTable(
                name: "RestaurantDescriptions",
                newName: "Description");

            migrationBuilder.RenameTable(
                name: "GeneralClasses",
                newName: "Class");

            migrationBuilder.RenameColumn(
                name: "TripSuggestionId",
                table: "Place",
                newName: "NumOfMembers");

            migrationBuilder.RenameIndex(
                name: "IX_TourismAreas_ZoneId",
                table: "Place",
                newName: "IX_Place_ZoneId");

            migrationBuilder.RenameIndex(
                name: "IX_TourismAreas_DescriptionId",
                table: "Place",
                newName: "IX_Place_DescriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_TourismAreas_ClassId",
                table: "Place",
                newName: "IX_Place_ClassId");

            migrationBuilder.RenameIndex(
                name: "IX_TourismAreas_CategoryID",
                table: "Place",
                newName: "IX_Place_CategoryID");

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "UsersRatings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "UsersBookMarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Place",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location_Address",
                table: "Place",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Location_Latitude",
                table: "Place",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Location_Longitude",
                table: "Place",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumOfBeds",
                table: "Place",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialMediaLink",
                table: "Place",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Description",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Class",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Place",
                table: "Place",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Description",
                table: "Description",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Class",
                table: "Class",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BusinessSocialProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceCategoryId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    PlatformName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessSocialProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessSocialProfiles_PlaceCategories_PlaceCategoryId",
                        column: x => x.PlaceCategoryId,
                        principalTable: "PlaceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BusinessSocialProfiles_Place_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Place",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PlaceImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceCategoryId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaceImages_PlaceCategories_PlaceCategoryId",
                        column: x => x.PlaceCategoryId,
                        principalTable: "PlaceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PlaceImages_Place_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Place",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersRatings_PlaceId",
                table: "UsersRatings",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersBookMarks_PlaceId",
                table: "UsersBookMarks",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessSocialProfiles_PlaceCategoryId",
                table: "BusinessSocialProfiles",
                column: "PlaceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessSocialProfiles_PlaceId",
                table: "BusinessSocialProfiles",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceImages_PlaceCategoryId",
                table: "PlaceImages",
                column: "PlaceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceImages_PlaceId",
                table: "PlaceImages",
                column: "PlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Place_Class_ClassId",
                table: "Place",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Place_Description_DescriptionId",
                table: "Place",
                column: "DescriptionId",
                principalTable: "Description",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Place_PlaceCategories_CategoryID",
                table: "Place",
                column: "CategoryID",
                principalTable: "PlaceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Place_Zones_ZoneId",
                table: "Place",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantFoodCategories_Place_RestaurantId",
                table: "RestaurantFoodCategories",
                column: "RestaurantId",
                principalTable: "Place",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersBookMarks_Place_PlaceId",
                table: "UsersBookMarks",
                column: "PlaceId",
                principalTable: "Place",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRatings_Place_PlaceId",
                table: "UsersRatings",
                column: "PlaceId",
                principalTable: "Place",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
