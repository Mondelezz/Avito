using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avito.Migrations
{
    /// <inheritdoc />
    public partial class test3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Users_AdId",
                table: "Favorites");

            migrationBuilder.RenameColumn(
                name: "AdId",
                table: "Favorites",
                newName: "AdModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Favorites_AdId",
                table: "Favorites",
                newName: "IX_Favorites_AdModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Users_AdModelId",
                table: "Favorites",
                column: "AdModelId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Users_AdModelId",
                table: "Favorites");

            migrationBuilder.RenameColumn(
                name: "AdModelId",
                table: "Favorites",
                newName: "AdId");

            migrationBuilder.RenameIndex(
                name: "IX_Favorites_AdModelId",
                table: "Favorites",
                newName: "IX_Favorites_AdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Users_AdId",
                table: "Favorites",
                column: "AdId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
