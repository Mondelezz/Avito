using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avito.Migrations
{
    /// <inheritdoc />
    public partial class test9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PathFile",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PathFile",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Users");
        }
    }
}
