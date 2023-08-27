using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineClipboard.Migrations
{
    /// <inheritdoc />
    public partial class addOnlyViewableOnce : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "onlyViewableOnce",
                table: "Entry",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "onlyViewableOnce",
                table: "Entry");
        }
    }
}
