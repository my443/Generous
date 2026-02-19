using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Generous.Migrations
{
    /// <inheritdoc />
    public partial class CoreData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElementField_Field_FieldsId",
                schema: "app",
                table: "ElementField");

            migrationBuilder.DropForeignKey(
                name: "FK_Field_FieldType_FieldTypeId",
                schema: "app",
                table: "Field");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FieldType",
                schema: "app",
                table: "FieldType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Field",
                schema: "app",
                table: "Field");

            migrationBuilder.RenameTable(
                name: "FieldType",
                schema: "app",
                newName: "FieldTypes",
                newSchema: "app");

            migrationBuilder.RenameTable(
                name: "Field",
                schema: "app",
                newName: "Fields",
                newSchema: "app");

            migrationBuilder.RenameIndex(
                name: "IX_Field_FieldTypeId",
                schema: "app",
                table: "Fields",
                newName: "IX_Fields_FieldTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FieldTypes",
                schema: "app",
                table: "FieldTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fields",
                schema: "app",
                table: "Fields",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CoreData",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ElementId = table.Column<int>(type: "integer", nullable: false),
                    Data = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoreData_Elements_ElementId",
                        column: x => x.ElementId,
                        principalSchema: "app",
                        principalTable: "Elements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoreData_ElementId",
                schema: "app",
                table: "CoreData",
                column: "ElementId");

            migrationBuilder.AddForeignKey(
                name: "FK_ElementField_Fields_FieldsId",
                schema: "app",
                table: "ElementField",
                column: "FieldsId",
                principalSchema: "app",
                principalTable: "Fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fields_FieldTypes_FieldTypeId",
                schema: "app",
                table: "Fields",
                column: "FieldTypeId",
                principalSchema: "app",
                principalTable: "FieldTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElementField_Fields_FieldsId",
                schema: "app",
                table: "ElementField");

            migrationBuilder.DropForeignKey(
                name: "FK_Fields_FieldTypes_FieldTypeId",
                schema: "app",
                table: "Fields");

            migrationBuilder.DropTable(
                name: "CoreData",
                schema: "app");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FieldTypes",
                schema: "app",
                table: "FieldTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fields",
                schema: "app",
                table: "Fields");

            migrationBuilder.RenameTable(
                name: "FieldTypes",
                schema: "app",
                newName: "FieldType",
                newSchema: "app");

            migrationBuilder.RenameTable(
                name: "Fields",
                schema: "app",
                newName: "Field",
                newSchema: "app");

            migrationBuilder.RenameIndex(
                name: "IX_Fields_FieldTypeId",
                schema: "app",
                table: "Field",
                newName: "IX_Field_FieldTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FieldType",
                schema: "app",
                table: "FieldType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Field",
                schema: "app",
                table: "Field",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ElementField_Field_FieldsId",
                schema: "app",
                table: "ElementField",
                column: "FieldsId",
                principalSchema: "app",
                principalTable: "Field",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Field_FieldType_FieldTypeId",
                schema: "app",
                table: "Field",
                column: "FieldTypeId",
                principalSchema: "app",
                principalTable: "FieldType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
