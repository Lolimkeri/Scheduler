using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchedulerWork.Migrations
{
    public partial class ttr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PairToFGroup");

            migrationBuilder.DropTable(
                name: "Program_Subjects");

            migrationBuilder.DropTable(
                name: "ProgramToGroups");

            migrationBuilder.DropTable(
                name: "Pairs");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "CourseProgram");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Audiences");

            migrationBuilder.DropTable(
                name: "Subjects");
        }
    }
}
