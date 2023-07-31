using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoList.Infrastructure.Data.Migrations
{
    public partial class Rate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rate",
                columns: table => new
                {
                    RateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstStar = table.Column<int>(type: "int", nullable: false),
                    SecondStar = table.Column<int>(type: "int", nullable: false),
                    ThirdStar = table.Column<int>(type: "int", nullable: false),
                    TaskFK = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rate", x => x.RateId);
                    table.ForeignKey(
                        name: "FK_Rate_DoneTasks_TaskFK",
                        column: x => x.TaskFK,
                        principalTable: "DoneTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rate_TaskFK",
                table: "Rate",
                column: "TaskFK",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rate");
        }
    }
}
