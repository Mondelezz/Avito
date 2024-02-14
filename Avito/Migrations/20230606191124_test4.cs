using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avito.Migrations
{
    /// <inheritdoc />
    public partial class test4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AdModels_adModelId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Users_AdModelId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_AdModelId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "AdModelId",
                table: "Favorites");

            migrationBuilder.RenameColumn(
                name: "adModelId",
                table: "Favorites",
                newName: "AdModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Favorites_adModelId",
                table: "Favorites",
                newName: "IX_Favorites_AdModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AdModels_AdModelId",
                table: "Favorites",
                column: "AdModelId",
                principalTable: "AdModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Users_UserId",
                table: "Favorites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AdModels_AdModelId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Users_UserId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites");

            migrationBuilder.RenameColumn(
                name: "AdModelId",
                table: "Favorites",
                newName: "adModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Favorites_AdModelId",
                table: "Favorites",
                newName: "IX_Favorites_adModelId");

            migrationBuilder.AddColumn<int>(
                name: "AdModelId",
                table: "Favorites",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_AdModelId",
                table: "Favorites",
                column: "AdModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AdModels_adModelId",
                table: "Favorites",
                column: "adModelId",
                principalTable: "AdModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Users_AdModelId",
                table: "Favorites",
                column: "AdModelId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
