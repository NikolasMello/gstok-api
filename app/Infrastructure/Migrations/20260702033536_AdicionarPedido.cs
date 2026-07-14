using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarVenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "venda",
                columns: table => new
                {
                    id_venda = table.Column<Guid>(type: "uuid", nullable: false),
                    cliente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    st_venda = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    st_pagamento = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    tp_pagamento = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    vl_subtotal = table.Column<decimal>(type: "numeric", nullable: false),
                    vl_frete = table.Column<decimal>(type: "numeric", nullable: false),
                    vl_desconto = table.Column<decimal>(type: "numeric", nullable: false),
                    vl_total = table.Column<decimal>(type: "numeric", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venda", x => x.id_venda);
                    table.ForeignKey(
                        name: "FK_venda_cliente_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "cliente",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "item_venda",
                columns: table => new
                {
                    id_item_venda = table.Column<Guid>(type: "uuid", nullable: false),
                    venda_id = table.Column<Guid>(type: "uuid", nullable: false),
                    estoque_id = table.Column<Guid>(type: "uuid", nullable: false),
                    qt_quantidade = table.Column<int>(type: "integer", nullable: false),
                    vl_unitario = table.Column<decimal>(type: "numeric", nullable: false),
                    vl_total = table.Column<decimal>(type: "numeric", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_venda", x => x.id_item_venda);
                    table.ForeignKey(
                        name: "FK_item_venda_estoque_estoque_id",
                        column: x => x.estoque_id,
                        principalTable: "estoque",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_venda_venda_venda_id",
                        column: x => x.venda_id,
                        principalTable: "venda",
                        principalColumn: "id_venda",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_item_venda_estoque_id",
                table: "item_venda",
                column: "estoque_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_venda_venda_id",
                table: "item_venda",
                column: "venda_id");

            migrationBuilder.CreateIndex(
                name: "IX_venda_cliente_id",
                table: "venda",
                column: "cliente_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item_venda");

            migrationBuilder.DropTable(
                name: "venda");
        }
    }
}
