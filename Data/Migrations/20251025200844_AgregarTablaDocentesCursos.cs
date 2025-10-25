using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablaDocentesCursos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "docentes_cursos",
                columns: table => new
                {
                    id_dictado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_curso = table.Column<int>(type: "int", nullable: false),
                    id_docente = table.Column<int>(type: "int", nullable: false),
                    cargo = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_docentes_cursos", x => x.id_dictado);
                    table.ForeignKey(
                        name: "FK_docentes_cursos_Cursos_id_curso",
                        column: x => x.id_curso,
                        principalTable: "Cursos",
                        principalColumn: "IdCurso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_docentes_cursos_Personas_id_docente",
                        column: x => x.id_docente,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_docentes_cursos_id_curso_id_docente_cargo",
                table: "docentes_cursos",
                columns: new[] { "id_curso", "id_docente", "cargo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_docentes_cursos_id_docente",
                table: "docentes_cursos",
                column: "id_docente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "docentes_cursos");
        }
    }
}
