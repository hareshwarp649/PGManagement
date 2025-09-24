using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyManage.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUnit02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Properties_PropertiyId",
                table: "Units");

            migrationBuilder.RenameColumn(
                name: "PropertiyId",
                table: "Units",
                newName: "PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_Units_PropertiyId",
                table: "Units",
                newName: "IX_Units_PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Properties_PropertyId",
                table: "Units",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_Properties_PropertyId",
                table: "Units");

            migrationBuilder.RenameColumn(
                name: "PropertyId",
                table: "Units",
                newName: "PropertiyId");

            migrationBuilder.RenameIndex(
                name: "IX_Units_PropertyId",
                table: "Units",
                newName: "IX_Units_PropertiyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Properties_PropertiyId",
                table: "Units",
                column: "PropertiyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
