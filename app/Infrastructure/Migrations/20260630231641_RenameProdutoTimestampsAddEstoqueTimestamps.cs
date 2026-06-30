using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class RenameProdutoTimestampsAddEstoqueTimestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dt_criacao",
                table: "produto");

            migrationBuilder.DropColumn(
                name: "dt_edicao",
                table: "produto");

            migrationBuilder.AddColumn<DateTime>(
                name: "ts_criacao",
                table: "produto",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ts_edicao",
                table: "produto",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ts_criacao",
                table: "estoque",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ts_edicao",
                table: "estoque",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ts_criacao",
                table: "produto");

            migrationBuilder.DropColumn(
                name: "ts_edicao",
                table: "produto");

            migrationBuilder.DropColumn(
                name: "ts_criacao",
                table: "estoque");

            migrationBuilder.DropColumn(
                name: "ts_edicao",
                table: "estoque");

            migrationBuilder.AddColumn<DateOnly>(
                name: "dt_criacao",
                table: "produto",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "dt_edicao",
                table: "produto",
                type: "date",
                nullable: true);
        }
    }
}
