using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SknC.Web.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIngredientSkinWarning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotRecommendedFor",
                table: "Ingredients",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotRecommendedFor",
                table: "Ingredients");
        }
    }
}
