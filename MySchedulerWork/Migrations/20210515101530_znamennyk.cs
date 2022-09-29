using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchedulerWork.Migrations
{
    public partial class znamennyk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsEverWeek",
                table: "Pairs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEverWeek",
                table: "Pairs");
        }
    }
}
