using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SknC.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoPathToJournal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "JournalEntries",
                type: "TEXT",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "JournalEntries");
        }
    }
}
