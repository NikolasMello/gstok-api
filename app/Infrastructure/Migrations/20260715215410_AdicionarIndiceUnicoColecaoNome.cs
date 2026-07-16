using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarIndiceUnicoColecaoNome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_colecao_fornecedor_id",
                table: "colecao");

            migrationBuilder.CreateIndex(
                name: "IX_colecao_fornecedor_id_nm_colecao",
                table: "colecao",
                columns: new[] { "fornecedor_id", "nm_colecao" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_colecao_fornecedor_id_nm_colecao",
                table: "colecao");

            migrationBuilder.CreateIndex(
                name: "IX_colecao_fornecedor_id",
                table: "colecao",
                column: "fornecedor_id");
        }
    }
}
