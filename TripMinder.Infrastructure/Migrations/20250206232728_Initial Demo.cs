using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripMinder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDemo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Description",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Description", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodCategories", x => x.Id);
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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersHistory_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UsersImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersImages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UsersSocialProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PlatformName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersSocialProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersSocialProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Place",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionId = table.Column<int>(type: "int", nullable: false),
                    Location_Latitude = table.Column<double>(type: "float", nullable: true),
                    Location_Longitude = table.Column<double>(type: "float", nullable: true),
                    Location_Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SocialMediaLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
                    AveragePricePerAdult = table.Column<double>(type: "float", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    HasKidsArea = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    NumOfBeds = table.Column<int>(type: "int", nullable: true),
                    NumOfMembers = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Place", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Place_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Place_Description_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "Description",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Place_PlaceCategories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "PlaceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Place_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BusinessSocialProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    PlaceCategoryId = table.Column<int>(type: "int", nullable: false)
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
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    PlaceCategoryId = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_RestaurantFoodCategories_Place_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Place",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UsersBookMarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersBookMarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersBookMarks_Place_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Place",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UsersBookMarks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UsersRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Review = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersRatings_Place_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Place",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UsersRatings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessSocialProfiles_PlaceCategoryId",
                table: "BusinessSocialProfiles",
                column: "PlaceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessSocialProfiles_PlaceId",
                table: "BusinessSocialProfiles",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Place_CategoryID",
                table: "Place",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Place_ClassId",
                table: "Place",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Place_DescriptionId",
                table: "Place",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Place_ZoneId",
                table: "Place",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceImages_PlaceCategoryId",
                table: "PlaceImages",
                column: "PlaceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaceImages_PlaceId",
                table: "PlaceImages",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantFoodCategories_FoodCategoryId",
                table: "RestaurantFoodCategories",
                column: "FoodCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersBookMarks_PlaceId",
                table: "UsersBookMarks",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersBookMarks_UserId",
                table: "UsersBookMarks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersHistory_UserId",
                table: "UsersHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersImages_UserId",
                table: "UsersImages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersRatings_PlaceId",
                table: "UsersRatings",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersRatings_UserId",
                table: "UsersRatings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersSocialProfiles_UserId",
                table: "UsersSocialProfiles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessSocialProfiles");

            migrationBuilder.DropTable(
                name: "PlaceImages");

            migrationBuilder.DropTable(
                name: "RestaurantFoodCategories");

            migrationBuilder.DropTable(
                name: "UsersBookMarks");

            migrationBuilder.DropTable(
                name: "UsersHistory");

            migrationBuilder.DropTable(
                name: "UsersImages");

            migrationBuilder.DropTable(
                name: "UsersRatings");

            migrationBuilder.DropTable(
                name: "UsersSocialProfiles");

            migrationBuilder.DropTable(
                name: "FoodCategories");

            migrationBuilder.DropTable(
                name: "Place");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Class");

            migrationBuilder.DropTable(
                name: "Description");

            migrationBuilder.DropTable(
                name: "PlaceCategories");

            migrationBuilder.DropTable(
                name: "Zones");
        }
    }
}
