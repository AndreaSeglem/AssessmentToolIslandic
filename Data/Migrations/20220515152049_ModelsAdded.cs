using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetterKnowledgeAssessment.Data.Migrations
{
    public partial class ModelsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ClassLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassLists_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pupils",
                columns: table => new
                {
                    PupilId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KnowledgeLevel = table.Column<int>(type: "int", nullable: false),
                    ClassListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pupils", x => x.PupilId);
                    table.ForeignKey(
                        name: "FK_Pupils_ClassLists_ClassListId",
                        column: x => x.ClassListId,
                        principalTable: "ClassLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LetterKnowledgeTestResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsUpperCase = table.Column<bool>(type: "bit", nullable: false),
                    PupilId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterKnowledgeTestResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LetterKnowledgeTestResults_Pupils_PupilId",
                        column: x => x.PupilId,
                        principalTable: "Pupils",
                        principalColumn: "PupilId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LetterSoundKnowledge",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Letter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KnowledgeLevel = table.Column<int>(type: "int", nullable: false),
                    TestResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterSoundKnowledge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LetterSoundKnowledge_LetterKnowledgeTestResults_TestResultId",
                        column: x => x.TestResultId,
                        principalTable: "LetterKnowledgeTestResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassLists_TeacherId",
                table: "ClassLists",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterKnowledgeTestResults_PupilId",
                table: "LetterKnowledgeTestResults",
                column: "PupilId");

            migrationBuilder.CreateIndex(
                name: "IX_LetterSoundKnowledge_TestResultId",
                table: "LetterSoundKnowledge",
                column: "TestResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Pupils_ClassListId",
                table: "Pupils",
                column: "ClassListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LetterSoundKnowledge");

            migrationBuilder.DropTable(
                name: "LetterKnowledgeTestResults");

            migrationBuilder.DropTable(
                name: "Pupils");

            migrationBuilder.DropTable(
                name: "ClassLists");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
