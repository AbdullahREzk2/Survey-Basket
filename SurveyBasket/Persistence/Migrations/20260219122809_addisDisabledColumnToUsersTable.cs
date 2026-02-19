using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.DAL.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addisDisabledColumnToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDisabled",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "98673a28-c8af-45c0-9efd-3f4af6e42529",
                column: "isDisabled",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDisabled",
                table: "AspNetUsers");
        }
    }
}
