using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregaModulosYPermisos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Habilitado",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateTable(
                name: "Modulos",
                columns: table => new
                {
                    Id_Modulo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Desc_Modulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ejecuta = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modulos", x => x.Id_Modulo);
                });

            migrationBuilder.CreateTable(
                name: "ModulosUsuarios",
                columns: table => new
                {
                    Id_ModuloUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    ModuloId = table.Column<int>(type: "int", nullable: false),
                    alta = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    baja = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    modificacion = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    consulta = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulosUsuarios", x => x.Id_ModuloUsuario);
                    table.ForeignKey(
                        name: "FK_ModulosUsuarios_Modulos_ModuloId",
                        column: x => x.ModuloId,
                        principalTable: "Modulos",
                        principalColumn: "Id_Modulo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModulosUsuarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModulosUsuarios_ModuloId",
                table: "ModulosUsuarios",
                column: "ModuloId");

            migrationBuilder.CreateIndex(
                name: "IX_ModulosUsuarios_UsuarioId_ModuloId",
                table: "ModulosUsuarios",
                columns: new[] { "UsuarioId", "ModuloId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModulosUsuarios");

            migrationBuilder.DropTable(
                name: "Modulos");

            migrationBuilder.AlterColumn<bool>(
                name: "Habilitado",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);
        }
    }
}
