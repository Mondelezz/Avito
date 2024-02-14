using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avito.Migrations
{
    /// <inheritdoc />
    public partial class test5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AdModels_AdModelId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_AdModelId",
                table: "Favorites");

            migrationBuilder.AddColumn<int>(
                name: "FavoriteModelId",
                table: "AdModels",
                type: "integer",
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdModels_Favorites_FavoriteModelId",
                table: "AdModels");

            migrationBuilder.DropIndex(
                name: "IX_AdModels_FavoriteModelId",
                table: "AdModels");

            migrationBuilder.DropColumn(
                name: "FavoriteModelId",
                table: "AdModels");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_AdModelId",
                table: "Favorites",
                column: "AdModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AdModels_AdModelId",
                table: "Favorites",
                column: "AdModelId",
                principalTable: "AdModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
