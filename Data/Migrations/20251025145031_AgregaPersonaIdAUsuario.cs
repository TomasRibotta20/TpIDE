using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregaPersonaIdAUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonaId",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_PersonaId",
                table: "Usuarios",
                column: "PersonaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Personas_PersonaId",
                table: "Usuarios",
                column: "PersonaId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Personas_PersonaId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_PersonaId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "PersonaId",
                table: "Usuarios");
        }
    }
}
