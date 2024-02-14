using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Avito.Migrations
{
    /// <inheritdoc />
    public partial class test6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdModels_Favorites_FavoriteModelId",
                table: "AdModels");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Users_UserId",
                table: "Favorites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_AdModels_FavoriteModelId",
                table: "AdModels");

            migrationBuilder.DropColumn(
                name: "FavoriteModelId",
                table: "AdModels");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Favorites",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites",
                columns: new[] { "UserId", "AdModelId" });

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
                name: "IX_Favorites_AdModelId",
                table: "Favorites",
                column: "AdModelId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteModelPerson_FavoritesUserId_FavoritesAdModelId",
                table: "FavoriteModelPerson",
                columns: new[] { "FavoritesUserId", "FavoritesAdModelId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AdModels_AdModelId",
                table: "Favorites",
                column: "AdModelId",
                principalTable: "AdModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AdModels_AdModelId",
                table: "Favorites");

            migrationBuilder.DropTable(
                name: "FavoriteModelPerson");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_AdModelId",
                table: "Favorites");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Favorites",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "FavoriteModelId",
                table: "AdModels",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdModels_FavoriteModelId",
                table: "AdModels",
                column: "FavoriteModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdModels_Favorites_FavoriteModelId",
                table: "AdModels",
                column: "FavoriteModelId",
                principalTable: "Favorites",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Users_UserId",
                table: "Favorites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
