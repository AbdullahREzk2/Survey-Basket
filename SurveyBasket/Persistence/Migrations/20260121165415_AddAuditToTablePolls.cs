using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.DAL.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditToTablePolls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "createdById",
                table: "Polls",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "createdOn",
                table: "Polls",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "updatedById",
                table: "Polls",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedOn",
                table: "Polls",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Polls_createdById",
                table: "Polls",
                column: "createdById");

            migrationBuilder.CreateIndex(
                name: "IX_Polls_updatedById",
                table: "Polls",
                column: "updatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_AspNetUsers_createdById",
                table: "Polls",
                column: "createdById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_AspNetUsers_updatedById",
                table: "Polls",
                column: "updatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_AspNetUsers_createdById",
                table: "Polls");

            migrationBuilder.DropForeignKey(
                name: "FK_Polls_AspNetUsers_updatedById",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Polls_createdById",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Polls_updatedById",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "createdById",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "createdOn",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "updatedById",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "updatedOn",
                table: "Polls");
        }
    }
}
