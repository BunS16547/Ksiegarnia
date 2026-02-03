using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ksiegarnia.Migrations
{
    /// <inheritdoc />
    public partial class DeletedbookpropertyIsAvailableForRentitiscalculatedinruntime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailableForRent",
                schema: "ksiegarnia_schema",
                table: "Books");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailableForRent",
                schema: "ksiegarnia_schema",
                table: "Books",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
