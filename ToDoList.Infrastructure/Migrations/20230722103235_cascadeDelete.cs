using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Infrastructure.Data.Migrations
{
    public partial class cascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Steps_ActiveTasks_TaskFK",
                table: "Steps");

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_ActiveTasks_TaskFK",
                table: "Steps",
                column: "TaskFK",
                principalTable: "ActiveTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Steps_ActiveTasks_TaskFK",
                table: "Steps");

            migrationBuilder.AddForeignKey(
                name: "FK_Steps_ActiveTasks_TaskFK",
                table: "Steps",
                column: "TaskFK",
                principalTable: "ActiveTasks",
                principalColumn: "Id");
        }
    }
}
