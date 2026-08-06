using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCompra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "compra",
                columns: table => new
                {
                    id_compra = table.Column<Guid>(type: "uuid", nullable: false),
                    fornecedor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    st_compra = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    dt_compra = table.Column<DateOnly>(type: "date", nullable: false),
                    dt_previsao_entrega = table.Column<DateOnly>(type: "date", nullable: true),
                    dt_recebimento = table.Column<DateOnly>(type: "date", nullable: true),
                    vl_total = table.Column<decimal>(type: "numeric", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_compra", x => x.id_compra);
                    table.ForeignKey(
                        name: "FK_compra_fornecedor_fornecedor_id",
                        column: x => x.fornecedor_id,
                        principalTable: "fornecedor",
                        principalColumn: "id_fornecedor",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "item_compra",
                columns: table => new
                {
                    id_item_compra = table.Column<Guid>(type: "uuid", nullable: false),
                    compra_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cor_produto_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tp_tamanho = table.Column<string>(type: "text", nullable: false),
                    qt_quantidade = table.Column<int>(type: "integer", nullable: false),
                    vl_unitario = table.Column<decimal>(type: "numeric", nullable: false),
                    vl_total = table.Column<decimal>(type: "numeric", nullable: false),
                    estoque_id = table.Column<Guid>(type: "uuid", nullable: true),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_compra", x => x.id_item_compra);
                    table.ForeignKey(
                        name: "FK_item_compra_compra_compra_id",
                        column: x => x.compra_id,
                        principalTable: "compra",
                        principalColumn: "id_compra",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_compra_cor_produto_cor_produto_id",
                        column: x => x.cor_produto_id,
                        principalTable: "cor_produto",
                        principalColumn: "id_cor_produto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_compra_estoque_estoque_id",
                        column: x => x.estoque_id,
                        principalTable: "estoque",
                        principalColumn: "id_estoque",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_compra_fornecedor_id",
                table: "compra",
                column: "fornecedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_compra_compra_id",
                table: "item_compra",
                column: "compra_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_compra_cor_produto_id",
                table: "item_compra",
                column: "cor_produto_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_compra_estoque_id",
                table: "item_compra",
                column: "estoque_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item_compra");

            migrationBuilder.DropTable(
                name: "compra");
        }
    }
}
