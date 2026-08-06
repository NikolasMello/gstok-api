using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarPromocao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "promocao",
                columns: table => new
                {
                    id_promocao = table.Column<Guid>(type: "uuid", nullable: false),
                    nm_promocao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    dt_inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    dt_termino = table.Column<DateOnly>(type: "date", nullable: false),
                    fl_ativo = table.Column<bool>(type: "boolean", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promocao", x => x.id_promocao);
                });

            migrationBuilder.CreateTable(
                name: "promocao_produto",
                columns: table => new
                {
                    id_promocao_produto = table.Column<Guid>(type: "uuid", nullable: false),
                    promocao_id = table.Column<Guid>(type: "uuid", nullable: false),
                    produto_id = table.Column<Guid>(type: "uuid", nullable: false),
                    pc_desconto = table.Column<decimal>(type: "numeric", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promocao_produto", x => x.id_promocao_produto);
                    table.ForeignKey(
                        name: "FK_promocao_produto_produto_produto_id",
                        column: x => x.produto_id,
                        principalTable: "produto",
                        principalColumn: "id_produto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_promocao_produto_promocao_promocao_id",
                        column: x => x.promocao_id,
                        principalTable: "promocao",
                        principalColumn: "id_promocao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_promocao_produto_produto_id",
                table: "promocao_produto",
                column: "produto_id");

            migrationBuilder.CreateIndex(
                name: "IX_promocao_produto_promocao_id_produto_id",
                table: "promocao_produto",
                columns: new[] { "promocao_id", "produto_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "promocao_produto");

            migrationBuilder.DropTable(
                name: "promocao");
        }
    }
}
