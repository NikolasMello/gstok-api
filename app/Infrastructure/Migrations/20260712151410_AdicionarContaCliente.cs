using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarContaCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "conta_cliente",
                columns: table => new
                {
                    id_conta_cliente = table.Column<Guid>(type: "uuid", nullable: false),
                    cliente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nm_email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ds_senha = table.Column<string>(type: "text", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conta_cliente", x => x.id_conta_cliente);
                    table.ForeignKey(
                        name: "FK_conta_cliente_cliente_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "cliente",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_conta_cliente_cliente_id",
                table: "conta_cliente",
                column: "cliente_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_conta_cliente_nm_email",
                table: "conta_cliente",
                column: "nm_email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "conta_cliente");
        }
    }
}
