using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarColecaoAoProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "colecao_id",
                table: "produto",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_produto_colecao_id",
                table: "produto",
                column: "colecao_id");

            migrationBuilder.AddForeignKey(
                name: "FK_produto_colecao_colecao_id",
                table: "produto",
                column: "colecao_id",
                principalTable: "colecao",
                principalColumn: "id_colecao",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_produto_colecao_colecao_id",
                table: "produto");

            migrationBuilder.DropIndex(
                name: "IX_produto_colecao_id",
                table: "produto");

            migrationBuilder.DropColumn(
                name: "colecao_id",
                table: "produto");
        }
    }
}
