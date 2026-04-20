using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorMat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialFieldsCorrect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PermiteSotckNegativo",
                table: "materiales",
                newName: "PermiteStockNegativo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PermiteStockNegativo",
                table: "materiales",
                newName: "PermiteSotckNegativo");
        }
    }
}
