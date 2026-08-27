using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarLojaOnline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "endereco_entrega_id",
                table: "venda",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tp_origem",
                table: "venda",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "Loja");

            migrationBuilder.CreateTable(
                name: "carrinho",
                columns: table => new
                {
                    id_carrinho = table.Column<Guid>(type: "uuid", nullable: false),
                    cliente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carrinho", x => x.id_carrinho);
                    table.ForeignKey(
                        name: "FK_carrinho_cliente_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "cliente",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "endereco",
                columns: table => new
                {
                    id_endereco = table.Column<Guid>(type: "uuid", nullable: false),
                    cliente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cd_cep = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    nm_logradouro = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    cd_numero = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ds_complemento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    nm_bairro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nm_cidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cd_uf = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    fl_principal = table.Column<bool>(type: "boolean", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_endereco", x => x.id_endereco);
                    table.ForeignKey(
                        name: "FK_endereco_cliente_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "cliente",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sessao_cliente",
                columns: table => new
                {
                    id_sessao_cliente = table.Column<Guid>(type: "uuid", nullable: false),
                    cliente_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cd_token = table.Column<string>(type: "text", nullable: false),
                    ts_expiracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessao_cliente", x => x.id_sessao_cliente);
                    table.ForeignKey(
                        name: "FK_sessao_cliente_cliente_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "cliente",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "carrinho_item",
                columns: table => new
                {
                    id_carrinho_item = table.Column<Guid>(type: "uuid", nullable: false),
                    carrinho_id = table.Column<Guid>(type: "uuid", nullable: false),
                    estoque_id = table.Column<Guid>(type: "uuid", nullable: false),
                    qt_quantidade = table.Column<int>(type: "integer", nullable: false),
                    ts_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ts_edicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carrinho_item", x => x.id_carrinho_item);
                    table.ForeignKey(
                        name: "FK_carrinho_item_carrinho_carrinho_id",
                        column: x => x.carrinho_id,
                        principalTable: "carrinho",
                        principalColumn: "id_carrinho",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_carrinho_item_estoque_estoque_id",
                        column: x => x.estoque_id,
                        principalTable: "estoque",
                        principalColumn: "id_estoque",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_venda_endereco_entrega_id",
                table: "venda",
                column: "endereco_entrega_id");

            migrationBuilder.CreateIndex(
                name: "IX_carrinho_cliente_id",
                table: "carrinho",
                column: "cliente_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_carrinho_item_carrinho_id_estoque_id",
                table: "carrinho_item",
                columns: new[] { "carrinho_id", "estoque_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_carrinho_item_estoque_id",
                table: "carrinho_item",
                column: "estoque_id");

            migrationBuilder.CreateIndex(
                name: "IX_endereco_cliente_id",
                table: "endereco",
                column: "cliente_id",
                unique: true,
                filter: "fl_principal = true");

            migrationBuilder.CreateIndex(
                name: "IX_sessao_cliente_cd_token",
                table: "sessao_cliente",
                column: "cd_token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sessao_cliente_cliente_id",
                table: "sessao_cliente",
                column: "cliente_id");

            migrationBuilder.AddForeignKey(
                name: "FK_venda_endereco_endereco_entrega_id",
                table: "venda",
                column: "endereco_entrega_id",
                principalTable: "endereco",
                principalColumn: "id_endereco",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_venda_endereco_endereco_entrega_id",
                table: "venda");

            migrationBuilder.DropTable(
                name: "carrinho_item");

            migrationBuilder.DropTable(
                name: "endereco");

            migrationBuilder.DropTable(
                name: "sessao_cliente");

            migrationBuilder.DropTable(
                name: "carrinho");

            migrationBuilder.DropIndex(
                name: "IX_venda_endereco_entrega_id",
                table: "venda");

            migrationBuilder.DropColumn(
                name: "endereco_entrega_id",
                table: "venda");

            migrationBuilder.DropColumn(
                name: "tp_origem",
                table: "venda");
        }
    }
}
