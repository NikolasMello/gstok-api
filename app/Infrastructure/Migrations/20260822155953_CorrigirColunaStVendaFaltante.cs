using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirColunaStVendaFaltante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // A migration RenomearPedidoParaVenda deixou a coluna "st_pedido" para trás
            // (deveria ter virado "st_venda" junto com o resto do rename tabela/model).
            migrationBuilder.RenameColumn(
                name: "st_pedido",
                table: "venda",
                newName: "st_venda");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "st_venda",
                table: "venda",
                newName: "st_pedido");
        }
    }
}
