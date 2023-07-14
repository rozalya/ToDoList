using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Infrastructure.Data.Migrations
{
    public partial class CloseTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "NewTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "NewTasks");
        }
    }
}
