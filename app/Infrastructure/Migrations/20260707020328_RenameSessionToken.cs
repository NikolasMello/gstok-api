using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gstok_api.Migrations
{
    /// <inheritdoc />
    public partial class RenameSessionToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cd_refresh_token",
                table: "sessao",
                newName: "cd_token");

            migrationBuilder.RenameIndex(
                name: "IX_sessao_cd_refresh_token",
                table: "sessao",
                newName: "IX_sessao_cd_token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cd_token",
                table: "sessao",
                newName: "cd_refresh_token");

            migrationBuilder.RenameIndex(
                name: "IX_sessao_cd_token",
                table: "sessao",
                newName: "IX_sessao_cd_refresh_token");
        }
    }
}
