using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class RenameImagemProdutoFullToAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ur_full",
                table: "imagem_produto",
                newName: "ur_avatar");

            migrationBuilder.RenameColumn(
                name: "nr_largura_full",
                table: "imagem_produto",
                newName: "nr_largura_avatar");

            migrationBuilder.RenameColumn(
                name: "nr_altura_full",
                table: "imagem_produto",
                newName: "nr_altura_avatar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ur_avatar",
                table: "imagem_produto",
                newName: "ur_full");

            migrationBuilder.RenameColumn(
                name: "nr_largura_avatar",
                table: "imagem_produto",
                newName: "nr_largura_full");

            migrationBuilder.RenameColumn(
                name: "nr_altura_avatar",
                table: "imagem_produto",
                newName: "nr_altura_full");
        }
    }
}
