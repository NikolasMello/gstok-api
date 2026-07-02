using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class AddImagemProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "imagem_produto",
                columns: table => new
                {
                    id_imagem_produto = table.Column<Guid>(type: "uuid", nullable: false),
                    produto_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nm_caption = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    nr_ordem = table.Column<int>(type: "integer", nullable: false),
                    fl_principal = table.Column<bool>(type: "boolean", nullable: false),
                    ur_thumbnail = table.Column<string>(type: "text", nullable: false),
                    nr_largura_thumbnail = table.Column<int>(type: "integer", nullable: false),
                    nr_altura_thumbnail = table.Column<int>(type: "integer", nullable: false),
                    ur_mobile = table.Column<string>(type: "text", nullable: false),
                    nr_largura_mobile = table.Column<int>(type: "integer", nullable: false),
                    nr_altura_mobile = table.Column<int>(type: "integer", nullable: false),
                    ur_tablet = table.Column<string>(type: "text", nullable: false),
                    nr_largura_tablet = table.Column<int>(type: "integer", nullable: false),
                    nr_altura_tablet = table.Column<int>(type: "integer", nullable: false),
                    ur_desktop = table.Column<string>(type: "text", nullable: false),
                    nr_largura_desktop = table.Column<int>(type: "integer", nullable: false),
                    nr_altura_desktop = table.Column<int>(type: "integer", nullable: false),
                    ur_full = table.Column<string>(type: "text", nullable: false),
                    nr_largura_full = table.Column<int>(type: "integer", nullable: false),
                    nr_altura_full = table.Column<int>(type: "integer", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_imagem_produto", x => x.id_imagem_produto);
                    table.ForeignKey(
                        name: "FK_imagem_produto_produto_produto_id",
                        column: x => x.produto_id,
                        principalTable: "produto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_imagem_produto_produto_id",
                table: "imagem_produto",
                column: "produto_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "imagem_produto");
        }
    }
}
