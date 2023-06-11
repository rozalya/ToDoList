using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Infrastructure.Data.Migrations
{
    public partial class datetime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NewTasks_Id",
                table: "NewTasks");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "NewTasks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<bool>(
                name: "IsImportant",
                table: "NewTasks",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "NewTasks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "NewTasks",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsImportant",
                table: "NewTasks",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "DueDate",
                table: "NewTasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewTasks_Id",
                table: "NewTasks",
                column: "Id",
                unique: true);
        }
    }
}
