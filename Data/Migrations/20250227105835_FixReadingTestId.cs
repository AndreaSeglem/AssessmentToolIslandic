using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetterKnowledgeAssessment.Data.Migrations
{
    public partial class FixReadingTestId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "ReadingTests",
                newName: "Date");

            migrationBuilder.AlterColumn<Guid>(
                name: "PupilId",
                table: "ReadingTests",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReadingTests",
                table: "ReadingTests");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ReadingTests");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ReadingTests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()"); // Setter en ny GUID automatisk

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReadingTests",
                table: "ReadingTests",
                column: "Id");


            migrationBuilder.AddColumn<bool>(
                name: "IsTestPassed",
                table: "ReadingTests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ReadingTestWord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsReadCorrectly = table.Column<bool>(type: "bit", nullable: false),
                    ReadingTestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingTestWord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadingTestWord_ReadingTests_ReadingTestId",
                        column: x => x.ReadingTestId,
                        principalTable: "ReadingTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReadingTests_PupilId",
                table: "ReadingTests",
                column: "PupilId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingTestWord_ReadingTestId",
                table: "ReadingTestWord",
                column: "ReadingTestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingTests_Pupils_PupilId",
                table: "ReadingTests",
                column: "PupilId",
                principalTable: "Pupils",
                principalColumn: "PupilId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReadingTests_Pupils_PupilId",
                table: "ReadingTests");

            migrationBuilder.DropTable(
                name: "ReadingTestWord");

            migrationBuilder.DropIndex(
                name: "IX_ReadingTests_PupilId",
                table: "ReadingTests");

            migrationBuilder.DropColumn(
                name: "IsTestPassed",
                table: "ReadingTests");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "ReadingTests",
                newName: "StartTime");

            migrationBuilder.AlterColumn<string>(
                name: "PupilId",
                table: "ReadingTests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ReadingTests",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
