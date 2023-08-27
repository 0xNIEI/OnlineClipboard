using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineClipboard.Migrations
{
    /// <inheritdoc />
    public partial class AddExpiresInTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "expires_at",
                table: "Entry",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "expires_at",
                table: "Entry");
        }
    }
}
