using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Generous.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsAndMore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldType",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Field",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    FixedColumnName = table.Column<string>(type: "text", nullable: false),
                    FieldTypeId = table.Column<int>(type: "integer", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Field", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Field_FieldType_FieldTypeId",
                        column: x => x.FieldTypeId,
                        principalSchema: "app",
                        principalTable: "FieldType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        name: "FK_ElementField_Field_FieldsId",
                        column: x => x.FieldsId,
                        principalSchema: "app",
                        principalTable: "Field",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElementField_FieldsId",
                schema: "app",
                table: "ElementField",
                column: "FieldsId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_FieldTypeId",
                schema: "app",
                table: "Field",
                column: "FieldTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElementField",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Field",
                schema: "app");

            migrationBuilder.DropTable(
                name: "FieldType",
                schema: "app");
        }
    }
}
