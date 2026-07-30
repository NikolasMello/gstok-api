using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class RemoverProdutoIdDoEstoqueEAdicionarIndiceUnico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_estoque_produto_produto_id",
                table: "estoque");

            migrationBuilder.DropIndex(
                name: "IX_estoque_cor_produto_id",
                table: "estoque");

            migrationBuilder.DropIndex(
                name: "IX_estoque_produto_id",
                table: "estoque");

            migrationBuilder.DropColumn(
                name: "produto_id",
                table: "estoque");

            migrationBuilder.CreateIndex(
                name: "IX_estoque_cor_produto_id_tp_tamanho",
                table: "estoque",
                columns: new[] { "cor_produto_id", "tp_tamanho" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_estoque_cor_produto_id_tp_tamanho",
                table: "estoque");

            migrationBuilder.AddColumn<Guid>(
                name: "produto_id",
                table: "estoque",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_estoque_cor_produto_id",
                table: "estoque",
                column: "cor_produto_id");

            migrationBuilder.CreateIndex(
                name: "IX_estoque_produto_id",
                table: "estoque",
                column: "produto_id");

            migrationBuilder.AddForeignKey(
                name: "FK_estoque_produto_produto_id",
                table: "estoque",
                column: "produto_id",
                principalTable: "produto",
                principalColumn: "id_produto",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
