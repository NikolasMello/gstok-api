using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCorProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nm_cor",
                table: "estoque");

            migrationBuilder.AddColumn<Guid>(
                name: "cor_produto_id",
                table: "estoque",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "cor_produto",
                columns: table => new
                {
                    id_cor_produto = table.Column<Guid>(type: "uuid", nullable: false),
                    produto_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nm_cor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    cd_hex = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cor_produto", x => x.id_cor_produto);
                    table.ForeignKey(
                        name: "FK_cor_produto_produto_produto_id",
                        column: x => x.produto_id,
                        principalTable: "produto",
                        principalColumn: "id_produto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_estoque_cor_produto_id",
                table: "estoque",
                column: "cor_produto_id");

            migrationBuilder.CreateIndex(
                name: "IX_cor_produto_produto_id_nm_cor",
                table: "cor_produto",
                columns: new[] { "produto_id", "nm_cor" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_estoque_cor_produto_cor_produto_id",
                table: "estoque",
                column: "cor_produto_id",
                principalTable: "cor_produto",
                principalColumn: "id_cor_produto",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_estoque_cor_produto_cor_produto_id",
                table: "estoque");

            migrationBuilder.DropTable(
                name: "cor_produto");

            migrationBuilder.DropIndex(
                name: "IX_estoque_cor_produto_id",
                table: "estoque");

            migrationBuilder.DropColumn(
                name: "cor_produto_id",
                table: "estoque");

            migrationBuilder.AddColumn<string>(
                name: "nm_cor",
                table: "estoque",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
