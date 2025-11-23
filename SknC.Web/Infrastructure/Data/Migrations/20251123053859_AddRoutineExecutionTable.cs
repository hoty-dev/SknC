using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SknC.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoutineExecutionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoutineExecutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoutineId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateExecuted = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DailyNotes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutineExecutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoutineExecutions_Routines_RoutineId",
                        column: x => x.RoutineId,
                        principalTable: "Routines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoutineExecutions_RoutineId",
                table: "RoutineExecutions",
                column: "RoutineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoutineExecutions");
        }
    }
}
