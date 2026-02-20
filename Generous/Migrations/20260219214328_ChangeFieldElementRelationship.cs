using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Generous.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFieldElementRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElementField",
                schema: "app");

            migrationBuilder.AddColumn<int>(
                name: "ElementId",
                schema: "app",
                table: "Fields",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fields_ElementId",
                schema: "app",
                table: "Fields",
                column: "ElementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_Elements_ElementId",
                schema: "app",
                table: "Fields",
                column: "ElementId",
                principalSchema: "app",
                principalTable: "Elements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fields_Elements_ElementId",
                schema: "app",
                table: "Fields");

            migrationBuilder.DropIndex(
                name: "IX_Fields_ElementId",
                schema: "app",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "ElementId",
                schema: "app",
                table: "Fields");

            migrationBuilder.CreateTable(
                name: "ElementField",
                schema: "app",
                columns: table => new
                {
                    ElementsId = table.Column<int>(type: "integer", nullable: false),
                    FieldsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementField", x => new { x.ElementsId, x.FieldsId });
                    table.ForeignKey(
                        name: "FK_ElementField_Elements_ElementsId",
                        column: x => x.ElementsId,
                        principalSchema: "app",
                        principalTable: "Elements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElementField_Fields_FieldsId",
                        column: x => x.FieldsId,
                        principalSchema: "app",
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElementField_FieldsId",
                schema: "app",
                table: "ElementField",
                column: "FieldsId");
        }
    }
}
