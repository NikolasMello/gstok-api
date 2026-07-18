using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class TornarTipoProdutoIdObrigatorio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_produto_tipo_produto_tipo_produto_id",
                table: "produto");

            migrationBuilder.AlterColumn<Guid>(
                name: "tipo_produto_id",
                table: "produto",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.Sql("ALTER TABLE produto ALTER COLUMN tipo_produto_id DROP DEFAULT;");

            migrationBuilder.AddForeignKey(
                name: "FK_produto_tipo_produto_tipo_produto_id",
                table: "produto",
                column: "tipo_produto_id",
                principalTable: "tipo_produto",
                principalColumn: "id_tipo_produto",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_produto_tipo_produto_tipo_produto_id",
                table: "produto");

            migrationBuilder.AlterColumn<Guid>(
                name: "tipo_produto_id",
                table: "produto",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_produto_tipo_produto_tipo_produto_id",
                table: "produto",
                column: "tipo_produto_id",
                principalTable: "tipo_produto",
                principalColumn: "id_tipo_produto",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
