using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Infrastructure.Data.Migrations
{
    public partial class NewStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_activeTasks",
                table: "activeTasks");

            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "activeTasks");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "activeTasks");

            migrationBuilder.RenameTable(
                name: "activeTasks",
                newName: "ActiveTasks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveTasks",
                table: "ActiveTasks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DoneTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsImportant = table.Column<bool>(type: "bit", nullable: false),
                    ClosingStatus = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoneTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpiderTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsImportant = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpiderTasks", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoneTasks");

            migrationBuilder.DropTable(
                name: "ExpiderTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveTasks",
                table: "ActiveTasks");

            migrationBuilder.RenameTable(
                name: "ActiveTasks",
                newName: "activeTasks");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDate",
                table: "activeTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "activeTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_activeTasks",
                table: "activeTasks",
                column: "Id");
        }
    }
}
