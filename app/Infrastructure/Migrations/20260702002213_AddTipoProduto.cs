using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "tipo_produto_id",
                table: "produto",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tipo_produto",
                columns: table => new
                {
                    id_tipo_produto = table.Column<Guid>(type: "uuid", nullable: false),
                    nm_tipo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_produto", x => x.id_tipo_produto);
                });

            migrationBuilder.CreateIndex(
                name: "IX_produto_tipo_produto_id",
                table: "produto",
                column: "tipo_produto_id");

            migrationBuilder.CreateIndex(
                name: "IX_tipo_produto_nm_tipo",
                table: "tipo_produto",
                column: "nm_tipo",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_produto_tipo_produto_tipo_produto_id",
                table: "produto",
                column: "tipo_produto_id",
                principalTable: "tipo_produto",
                principalColumn: "id_tipo_produto",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_produto_tipo_produto_tipo_produto_id",
                table: "produto");

            migrationBuilder.DropTable(
                name: "tipo_produto");

            migrationBuilder.DropIndex(
                name: "IX_produto_tipo_produto_id",
                table: "produto");

            migrationBuilder.DropColumn(
                name: "tipo_produto_id",
                table: "produto");
        }
    }
}
