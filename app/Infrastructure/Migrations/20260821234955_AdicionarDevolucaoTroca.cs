using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarDevolucaoTroca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "devolucao",
                columns: table => new
                {
                    id_devolucao = table.Column<Guid>(type: "uuid", nullable: false),
                    venda_id = table.Column<Guid>(type: "uuid", nullable: false),
                    st_devolucao = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ds_motivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    tp_reembolso = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    vl_total = table.Column<decimal>(type: "numeric", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devolucao", x => x.id_devolucao);
                    table.ForeignKey(
                        name: "FK_devolucao_venda_venda_id",
                        column: x => x.venda_id,
                        principalTable: "venda",
                        principalColumn: "id_venda",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "troca",
                columns: table => new
                {
                    id_troca = table.Column<Guid>(type: "uuid", nullable: false),
                    venda_id = table.Column<Guid>(type: "uuid", nullable: false),
                    st_troca = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ds_motivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    vl_total_saida = table.Column<decimal>(type: "numeric", nullable: false),
                    vl_total_entrada = table.Column<decimal>(type: "numeric", nullable: false),
                    vl_diferenca = table.Column<decimal>(type: "numeric", nullable: false),
                    tp_pagamento = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    tp_reembolso = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_troca", x => x.id_troca);
                    table.ForeignKey(
                        name: "FK_troca_venda_venda_id",
                        column: x => x.venda_id,
                        principalTable: "venda",
                        principalColumn: "id_venda",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "item_devolucao",
                columns: table => new
                {
                    id_item_devolucao = table.Column<Guid>(type: "uuid", nullable: false),
                    devolucao_id = table.Column<Guid>(type: "uuid", nullable: false),
                    venda_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    qt_quantidade = table.Column<int>(type: "integer", nullable: false),
                    vl_unitario = table.Column<decimal>(type: "numeric", nullable: false),
                    vl_total = table.Column<decimal>(type: "numeric", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_devolucao", x => x.id_item_devolucao);
                    table.ForeignKey(
                        name: "FK_item_devolucao_devolucao_devolucao_id",
                        column: x => x.devolucao_id,
                        principalTable: "devolucao",
                        principalColumn: "id_devolucao",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_item_devolucao_item_venda_venda_item_id",
                        column: x => x.venda_item_id,
                        principalTable: "item_venda",
                        principalColumn: "id_item_venda",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "item_troca_entrada",
                columns: table => new
                {
                    id_item_troca_entrada = table.Column<Guid>(type: "uuid", nullable: false),
                    troca_id = table.Column<Guid>(type: "uuid", nullable: false),
                    estoque_id = table.Column<Guid>(type: "uuid", nullable: false),
                    qt_quantidade = table.Column<int>(type: "integer", nullable: false),
                    vl_unitario = table.Column<decimal>(type: "numeric", nullable: false),
                    vl_total = table.Column<decimal>(type: "numeric", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_troca_entrada", x => x.id_item_troca_entrada);
                    table.ForeignKey(
                        name: "FK_item_troca_entrada_estoque_estoque_id",
                        column: x => x.estoque_id,
                        principalTable: "estoque",
                        principalColumn: "id_estoque",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_troca_entrada_troca_troca_id",
                        column: x => x.troca_id,
                        principalTable: "troca",
                        principalColumn: "id_troca",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "item_troca_saida",
                columns: table => new
                {
                    id_item_troca_saida = table.Column<Guid>(type: "uuid", nullable: false),
                    troca_id = table.Column<Guid>(type: "uuid", nullable: false),
                    venda_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    qt_quantidade = table.Column<int>(type: "integer", nullable: false),
                    vl_unitario = table.Column<decimal>(type: "numeric", nullable: false),
                    vl_total = table.Column<decimal>(type: "numeric", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_troca_saida", x => x.id_item_troca_saida);
                    table.ForeignKey(
                        name: "FK_item_troca_saida_item_venda_venda_item_id",
                        column: x => x.venda_item_id,
                        principalTable: "item_venda",
                        principalColumn: "id_item_venda",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_item_troca_saida_troca_troca_id",
                        column: x => x.troca_id,
                        principalTable: "troca",
                        principalColumn: "id_troca",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_devolucao_venda_id",
                table: "devolucao",
                column: "venda_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_devolucao_devolucao_id",
                table: "item_devolucao",
                column: "devolucao_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_devolucao_venda_item_id",
                table: "item_devolucao",
                column: "venda_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_troca_entrada_estoque_id",
                table: "item_troca_entrada",
                column: "estoque_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_troca_entrada_troca_id",
                table: "item_troca_entrada",
                column: "troca_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_troca_saida_troca_id",
                table: "item_troca_saida",
                column: "troca_id");

            migrationBuilder.CreateIndex(
                name: "IX_item_troca_saida_venda_item_id",
                table: "item_troca_saida",
                column: "venda_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_troca_venda_id",
                table: "troca",
                column: "venda_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item_devolucao");

            migrationBuilder.DropTable(
                name: "item_troca_entrada");

            migrationBuilder.DropTable(
                name: "item_troca_saida");

            migrationBuilder.DropTable(
                name: "devolucao");

            migrationBuilder.DropTable(
                name: "troca");
        }
    }
}
