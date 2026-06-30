using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class AddSessaoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id_usuario = table.Column<Guid>(type: "uuid", nullable: false),
                    nm_email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ds_senha = table.Column<string>(type: "text", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "sessao",
                columns: table => new
                {
                    id_sessao = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cd_refresh_token = table.Column<string>(type: "text", nullable: false),
                    ts_expiracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessao", x => x.id_sessao);
                    table.ForeignKey(
                        name: "FK_sessao_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sessao_cd_refresh_token",
                table: "sessao",
                column: "cd_refresh_token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sessao_usuario_id",
                table: "sessao",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_nm_email",
                table: "usuario",
                column: "nm_email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sessao");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
