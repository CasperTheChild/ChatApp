using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddContacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlockedUsers",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ContactUserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockedUsers", x => new { x.UserId, x.ContactUserId });
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ContactUserId = table.Column<string>(type: "text", nullable: false),
                    CustomName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => new { x.UserId, x.ContactUserId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_UserId_CustomName",
                table: "Contacts",
                columns: new[] { "UserId", "CustomName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockedUsers");

            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
