using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class LinkUsuarioPessoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "pessoa_id",
                table: "usuario",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_pessoa_id",
                table: "usuario",
                column: "pessoa_id",
                unique: true,
                filter: "pessoa_id IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_usuario_pessoa_pessoa_id",
                table: "usuario",
                column: "pessoa_id",
                principalTable: "pessoa",
                principalColumn: "id_pessoa",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_usuario_pessoa_pessoa_id",
                table: "usuario");

            migrationBuilder.DropIndex(
                name: "IX_usuario_pessoa_id",
                table: "usuario");

            migrationBuilder.DropColumn(
                name: "pessoa_id",
                table: "usuario");
        }
    }
}
