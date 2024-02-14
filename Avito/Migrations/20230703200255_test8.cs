using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avito.Migrations
{
    /// <inheritdoc />
    public partial class test8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteModelPerson");

            migrationBuilder.AddColumn<string>(
                name: "PathDirectory",
                table: "AdModels",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FavoriteModelPersons",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FavoritesUserId = table.Column<int>(type: "integer", nullable: false),
                    FavoritesAdModelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteModelPersons", x => new { x.UserId, x.FavoritesUserId, x.FavoritesAdModelId });
                    table.ForeignKey(
                        name: "FK_FavoriteModelPersons_Favorites_FavoritesUserId_FavoritesAdM~",
                        columns: x => new { x.FavoritesUserId, x.FavoritesAdModelId },
                        principalTable: "Favorites",
                        principalColumns: new[] { "UserId", "AdModelId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteModelPersons_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteModelPersons_FavoritesUserId_FavoritesAdModelId",
                table: "FavoriteModelPersons",
                columns: new[] { "FavoritesUserId", "FavoritesAdModelId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteModelPersons");

            migrationBuilder.DropColumn(
                name: "PathDirectory",
                table: "AdModels");

            migrationBuilder.CreateTable(
                name: "FavoriteModelPerson",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FavoritesUserId = table.Column<int>(type: "integer", nullable: false),
                    FavoritesAdModelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteModelPerson", x => new { x.UserId, x.FavoritesUserId, x.FavoritesAdModelId });
                    table.ForeignKey(
                        name: "FK_FavoriteModelPerson_Favorites_FavoritesUserId_FavoritesAdMo~",
                        columns: x => new { x.FavoritesUserId, x.FavoritesAdModelId },
                        principalTable: "Favorites",
                        principalColumns: new[] { "UserId", "AdModelId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteModelPerson_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteModelPerson_FavoritesUserId_FavoritesAdModelId",
                table: "FavoriteModelPerson",
                columns: new[] { "FavoritesUserId", "FavoritesAdModelId" });
        }
    }
}
