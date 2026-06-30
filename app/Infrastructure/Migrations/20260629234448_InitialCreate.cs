using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pessoa",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nr_cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    nm_pessoa = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nm_sobrenome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nm_telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    nm_email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pessoa", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "produto",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cd_sku = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nm_produto = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ds_produto = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    nm_marca = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    vl_preco = table.Column<decimal>(type: "numeric", nullable: false),
                    vl_venda = table.Column<decimal>(type: "numeric", nullable: false),
                    fl_ativo = table.Column<bool>(type: "boolean", nullable: false),
                    dt_criacao = table.Column<DateOnly>(type: "date", nullable: false),
                    dt_edicao = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produto", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "estoque",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    produto_id = table.Column<Guid>(type: "uuid", nullable: false),
                    qt_estoque = table.Column<int>(type: "integer", nullable: false),
                    tp_tamanho = table.Column<string>(type: "text", nullable: false),
                    nm_cor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estoque", x => x.id);
                    table.ForeignKey(
                        name: "FK_estoque_produto_produto_id",
                        column: x => x.produto_id,
                        principalTable: "produto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_estoque_produto_id",
                table: "estoque",
                column: "produto_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "estoque");

            migrationBuilder.DropTable(
                name: "pessoa");

            migrationBuilder.DropTable(
                name: "produto");
        }
    }
}
