using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarFornecedorColecao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "fornecedor",
                columns: table => new
                {
                    id_fornecedor = table.Column<Guid>(type: "uuid", nullable: false),
                    cd_cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    nm_empresa = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    nm_fantasia = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fornecedor", x => x.id_fornecedor);
                });

            migrationBuilder.CreateTable(
                name: "colecao",
                columns: table => new
                {
                    id_colecao = table.Column<Guid>(type: "uuid", nullable: false),
                    fornecedor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nm_colecao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_colecao", x => x.id_colecao);
                    table.ForeignKey(
                        name: "FK_colecao_fornecedor_fornecedor_id",
                        column: x => x.fornecedor_id,
                        principalTable: "fornecedor",
                        principalColumn: "id_fornecedor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_colecao_fornecedor_id",
                table: "colecao",
                column: "fornecedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_fornecedor_cd_cnpj",
                table: "fornecedor",
                column: "cd_cnpj",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "colecao");

            migrationBuilder.DropTable(
                name: "fornecedor");
        }
    }
}
