using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchedulerWork.Migrations
{
    public partial class deleteProgGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgramToGroups");

            migrationBuilder.AddColumn<int>(
                name: "CourseProgramId",
                table: "Groups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CourseProgramId",
                table: "Groups",
                column: "CourseProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_CourseProgram_CourseProgramId",
                table: "Groups",
                column: "CourseProgramId",
                principalTable: "CourseProgram",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_CourseProgram_CourseProgramId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_CourseProgramId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CourseProgramId",
                table: "Groups");

            migrationBuilder.CreateTable(
                name: "ProgramToGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseProgramId = table.Column<int>(type: "int", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramToGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramToGroups_CourseProgram_CourseProgramId",
                        column: x => x.CourseProgramId,
                        principalTable: "CourseProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramToGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgramToGroups_CourseProgramId",
                table: "ProgramToGroups",
                column: "CourseProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramToGroups_GroupId",
                table: "ProgramToGroups",
                column: "GroupId");
        }
    }
}
