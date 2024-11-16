using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farven.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetTokenToClientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetTokenExpires",
                table: "Clientes",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "PasswordResetTokenExpires",
                table: "Clientes");
        }
    }
}
