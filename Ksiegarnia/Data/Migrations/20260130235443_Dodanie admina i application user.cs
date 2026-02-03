using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ksiegarnia.Migrations
{
    /// <inheritdoc />
    public partial class Dodanieadminaiapplicationuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ksiegarnia_schema");

            migrationBuilder.RenameTable(
                name: "Isbns",
                newName: "Isbns",
                newSchema: "ksiegarnia_schema");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Categories",
                newSchema: "ksiegarnia_schema");

            migrationBuilder.RenameTable(
                name: "Books",
                newName: "Books",
                newSchema: "ksiegarnia_schema");

            migrationBuilder.RenameTable(
                name: "Authors",
                newName: "Authors",
                newSchema: "ksiegarnia_schema");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "ksiegarnia_schema");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AspNetUsers",
                newSchema: "ksiegarnia_schema");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "ksiegarnia_schema");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "ksiegarnia_schema");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "ksiegarnia_schema");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "ksiegarnia_schema");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "ksiegarnia_schema");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "ksiegarnia_schema",
                table: "AspNetUserTokens",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                schema: "ksiegarnia_schema",
                table: "AspNetUserTokens",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<bool>(
                name: "EnableNotifications",
                schema: "ksiegarnia_schema",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Initials",
                schema: "ksiegarnia_schema",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                schema: "ksiegarnia_schema",
                table: "AspNetUserLogins",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                schema: "ksiegarnia_schema",
                table: "AspNetUserLogins",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableNotifications",
                schema: "ksiegarnia_schema",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Initials",
                schema: "ksiegarnia_schema",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Isbns",
                schema: "ksiegarnia_schema",
                newName: "Isbns");

            migrationBuilder.RenameTable(
                name: "Categories",
                schema: "ksiegarnia_schema",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "Books",
                schema: "ksiegarnia_schema",
                newName: "Books");

            migrationBuilder.RenameTable(
                name: "Authors",
                schema: "ksiegarnia_schema",
                newName: "Authors");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "ksiegarnia_schema",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "ksiegarnia_schema",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "ksiegarnia_schema",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "ksiegarnia_schema",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "ksiegarnia_schema",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "ksiegarnia_schema",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "ksiegarnia_schema",
                newName: "AspNetRoleClaims");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);
        }
    }
}
