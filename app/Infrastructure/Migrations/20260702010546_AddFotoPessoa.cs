using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class AddFotoPessoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "foto_pessoa",
                columns: table => new
                {
                    id_foto_pessoa = table.Column<Guid>(type: "uuid", nullable: false),
                    pessoa_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nm_imagem = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ur_imagem = table.Column<string>(type: "text", nullable: false),
                    nr_largura = table.Column<int>(type: "integer", nullable: false),
                    nr_altura = table.Column<int>(type: "integer", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_foto_pessoa", x => x.id_foto_pessoa);
                    table.ForeignKey(
                        name: "FK_foto_pessoa_pessoa_pessoa_id",
                        column: x => x.pessoa_id,
                        principalTable: "pessoa",
                        principalColumn: "id_pessoa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_foto_pessoa_pessoa_id",
                table: "foto_pessoa",
                column: "pessoa_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "foto_pessoa");
        }
    }
}
