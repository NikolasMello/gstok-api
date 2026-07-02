using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class RenomearNrCpfParaCdInscricaoNacional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nr_cpf",
                table: "pessoa",
                newName: "cd_inscricao_nacional");

            migrationBuilder.AlterColumn<string>(
                name: "cd_inscricao_nacional",
                table: "pessoa",
                type: "character varying(14)",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11);

            migrationBuilder.CreateIndex(
                name: "IX_pessoa_cd_inscricao_nacional",
                table: "pessoa",
                column: "cd_inscricao_nacional",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_pessoa_cd_inscricao_nacional",
                table: "pessoa");

            migrationBuilder.AlterColumn<string>(
                name: "cd_inscricao_nacional",
                table: "pessoa",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(14)",
                oldMaxLength: 14);

            migrationBuilder.RenameColumn(
                name: "cd_inscricao_nacional",
                table: "pessoa",
                newName: "nr_cpf");
        }
    }
}
