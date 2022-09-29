using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchedulerWork.Migrations
{
    public partial class ttr1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pairs_Subjects_SubjectId",
                table: "Pairs");

            migrationBuilder.DropIndex(
                name: "IX_Pairs_SubjectId",
                table: "Pairs");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Pairs");

            migrationBuilder.AddColumn<int>(
                name: "Program_SubjectId",
                table: "Pairs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pairs_Program_SubjectId",
                table: "Pairs",
                column: "Program_SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pairs_Program_Subjects_Program_SubjectId",
                table: "Pairs",
                column: "Program_SubjectId",
                principalTable: "Program_Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pairs_Program_Subjects_Program_SubjectId",
                table: "Pairs");

            migrationBuilder.DropIndex(
                name: "IX_Pairs_Program_SubjectId",
                table: "Pairs");

            migrationBuilder.DropColumn(
                name: "Program_SubjectId",
                table: "Pairs");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Pairs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pairs_SubjectId",
                table: "Pairs",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pairs_Subjects_SubjectId",
                table: "Pairs",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
