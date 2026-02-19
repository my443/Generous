using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Generous.Migrations
{
    /// <inheritdoc />
    public partial class one1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.RenameTable(
                name: "Elements",
                newName: "Elements",
                newSchema: "app");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Elements",
                schema: "app",
                newName: "Elements");
        }
    }
}
