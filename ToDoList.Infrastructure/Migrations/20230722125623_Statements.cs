using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Infrastructure.Data.Migrations
{
    public partial class Statements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statements",
                columns: table => new
                {
                    StatementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    If = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Then = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TaskFK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statements", x => x.StatementId);
                    table.ForeignKey(
                        name: "FK_Statements_ActiveTasks_TaskFK",
                        column: x => x.TaskFK,
                        principalTable: "ActiveTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Statements_TaskFK",
                table: "Statements",
                column: "TaskFK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statements");
        }
    }
}
