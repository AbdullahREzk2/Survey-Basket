using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SurveyBasket.DAL.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionAndAnswerTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_AspNetUsers_createdById",
                table: "Polls");

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    questionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    PollId = table.Column<int>(type: "integer", nullable: false),
                    createdById = table.Column<string>(type: "text", nullable: false),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedById = table.Column<string>(type: "text", nullable: true),
                    updatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.questionId);
                    table.ForeignKey(
                        name: "FK_Questions_AspNetUsers_createdById",
                        column: x => x.createdById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Questions_AspNetUsers_updatedById",
                        column: x => x.updatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_Polls_PollId",
                        column: x => x.PollId,
                        principalTable: "Polls",
                        principalColumn: "PollId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    answerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    questionId = table.Column<int>(type: "integer", nullable: false),
                    createdById = table.Column<string>(type: "text", nullable: false),
                    createdOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedById = table.Column<string>(type: "text", nullable: true),
                    updatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.answerId);
                    table.ForeignKey(
                        name: "FK_Answers_AspNetUsers_createdById",
                        column: x => x.createdById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Answers_AspNetUsers_updatedById",
                        column: x => x.updatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Answers_Questions_questionId",
                        column: x => x.questionId,
                        principalTable: "Questions",
                        principalColumn: "questionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_createdById",
                table: "Answers",
                column: "createdById");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_questionId_Content",
                table: "Answers",
                columns: new[] { "questionId", "Content" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_updatedById",
                table: "Answers",
                column: "updatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_createdById",
                table: "Questions",
                column: "createdById");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_PollId_Content",
                table: "Questions",
                columns: new[] { "PollId", "Content" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_updatedById",
                table: "Questions",
                column: "updatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_AspNetUsers_createdById",
                table: "Polls",
                column: "createdById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_AspNetUsers_createdById",
                table: "Polls");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_AspNetUsers_createdById",
                table: "Polls",
                column: "createdById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
