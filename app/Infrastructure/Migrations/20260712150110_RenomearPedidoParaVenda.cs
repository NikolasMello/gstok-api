using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class RenomearVendaParaVenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remover FKs antes de renomear tabelas
            migrationBuilder.DropForeignKey(
                name: "FK_item_venda_venda_venda_id",
                table: "item_venda");

            migrationBuilder.DropForeignKey(
                name: "FK_item_venda_estoque_estoque_id",
                table: "item_venda");

            migrationBuilder.DropForeignKey(
                name: "FK_venda_cliente_cliente_id",
                table: "venda");

            // Remover índices antes de renomear
            migrationBuilder.DropIndex(
                name: "IX_item_venda_venda_id",
                table: "item_venda");

            migrationBuilder.DropIndex(
                name: "IX_item_venda_estoque_id",
                table: "item_venda");

            migrationBuilder.DropIndex(
                name: "IX_venda_cliente_id",
                table: "venda");

            // Renomear tabelas
            migrationBuilder.RenameTable(
                name: "item_venda",
                newName: "item_venda");

            migrationBuilder.RenameTable(
                name: "venda",
                newName: "venda");

            // Renomear colunas
            migrationBuilder.RenameColumn(
                name: "id_item_venda",
                table: "item_venda",
                newName: "id_item_venda");

            migrationBuilder.RenameColumn(
                name: "venda_id",
                table: "item_venda",
                newName: "venda_id");

            migrationBuilder.RenameColumn(
                name: "id_venda",
                table: "venda",
                newName: "id_venda");

            // Renomear PKs
            migrationBuilder.RenameIndex(
                name: "PK_venda",
                table: "venda",
                newName: "PK_venda");

            migrationBuilder.RenameIndex(
                name: "PK_item_venda",
                table: "item_venda",
                newName: "PK_item_venda");

            // Recriar índices com novos nomes
            migrationBuilder.CreateIndex(
                name: "IX_venda_cliente_id",
                table: "venda",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_venda_venda_id",
                table: "item_venda",
                column: "venda_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_venda_estoque_id",
                table: "item_venda",
                column: "estoque_id");

            // Recriar FKs com novos nomes
            migrationBuilder.AddForeignKey(
                name: "FK_venda_cliente_cliente_id",
                table: "venda",
                column: "cliente_id",
                principalTable: "cliente",
                principalColumn: "id_cliente",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_item_venda_venda_venda_id",
                table: "item_venda",
                column: "venda_id",
                principalTable: "venda",
                principalColumn: "id_venda",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_item_venda_estoque_estoque_id",
                table: "item_venda",
                column: "estoque_id",
                principalTable: "estoque",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_item_venda_venda_venda_id",
                table: "item_venda");

            migrationBuilder.DropForeignKey(
                name: "FK_item_venda_estoque_estoque_id",
                table: "item_venda");

            migrationBuilder.DropForeignKey(
                name: "FK_venda_cliente_cliente_id",
                table: "venda");

            migrationBuilder.DropIndex(
                name: "IX_item_venda_venda_id",
                table: "item_venda");

            migrationBuilder.DropIndex(
                name: "IX_item_venda_estoque_id",
                table: "item_venda");

            migrationBuilder.DropIndex(
                name: "IX_venda_cliente_id",
                table: "venda");

            migrationBuilder.RenameIndex(
                name: "PK_item_venda",
                table: "item_venda",
                newName: "PK_item_venda");

            migrationBuilder.RenameIndex(
                name: "PK_venda",
                table: "venda",
                newName: "PK_venda");

            migrationBuilder.RenameColumn(
                name: "id_venda",
                table: "venda",
                newName: "id_venda");

            migrationBuilder.RenameColumn(
                name: "venda_id",
                table: "item_venda",
                newName: "venda_id");

            migrationBuilder.RenameColumn(
                name: "id_item_venda",
                table: "item_venda",
                newName: "id_item_venda");

            migrationBuilder.RenameTable(
                name: "venda",
                newName: "venda");

            migrationBuilder.RenameTable(
                name: "item_venda",
                newName: "item_venda");

            migrationBuilder.CreateIndex(
                name: "IX_venda_cliente_id",
                table: "venda",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_venda_venda_id",
                table: "item_venda",
                column: "venda_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_venda_estoque_id",
                table: "item_venda",
                column: "estoque_id");

            migrationBuilder.AddForeignKey(
                name: "FK_venda_cliente_cliente_id",
                table: "venda",
                column: "cliente_id",
                principalTable: "cliente",
                principalColumn: "id_cliente",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_item_venda_venda_venda_id",
                table: "item_venda",
                column: "venda_id",
                principalTable: "venda",
                principalColumn: "id_venda",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_item_venda_estoque_estoque_id",
                table: "item_venda",
                column: "estoque_id",
                principalTable: "estoque",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
