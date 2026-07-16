using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class RenomearIdParaIdProdutoEIdEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "produto",
                newName: "id_produto");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "estoque",
                newName: "id_estoque");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id_produto",
                table: "produto",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id_estoque",
                table: "estoque",
                newName: "id");
        }
    }
}
