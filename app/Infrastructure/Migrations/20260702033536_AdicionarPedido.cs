using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pedido",
                columns: table => new
                {
                    id_pedido = table.Column<Guid>(type: "uuid", nullable: false),
                    cliente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    st_pedido = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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
                    table.PrimaryKey("PK_pedido", x => x.id_pedido);
                    table.ForeignKey(
                        name: "FK_pedido_cliente_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "cliente",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "item_pedido",
                columns: table => new
                {
                    id_item_pedido = table.Column<Guid>(type: "uuid", nullable: false),
                    pedido_id = table.Column<Guid>(type: "uuid", nullable: false),
                    estoque_id = table.Column<Guid>(type: "uuid", nullable: false),
                    qt_quantidade = table.Column<int>(type: "integer", nullable: false),
                    vl_unitario = table.Column<decimal>(type: "numeric", nullable: false),
                    vl_total = table.Column<decimal>(type: "numeric", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_pedido", x => x.id_item_pedido);
                    table.ForeignKey(
                        name: "FK_item_pedido_estoque_estoque_id",
                        column: x => x.estoque_id,
                        principalTable: "estoque",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_pedido_pedido_pedido_id",
                        column: x => x.pedido_id,
                        principalTable: "pedido",
                        principalColumn: "id_pedido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_item_pedido_estoque_id",
                table: "item_pedido",
                column: "estoque_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_pedido_pedido_id",
                table: "item_pedido",
                column: "pedido_id");

            migrationBuilder.CreateIndex(
                name: "IX_pedido_cliente_id",
                table: "pedido",
                column: "cliente_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item_pedido");

            migrationBuilder.DropTable(
                name: "pedido");
        }
    }
}
