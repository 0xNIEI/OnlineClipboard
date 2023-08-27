using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineClipboard.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToCustomId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Entry_custom_id",
                table: "Entry",
                column: "custom_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Entry_custom_id",
                table: "Entry");
        }
    }
}
