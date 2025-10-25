using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregaCursosYAlumnoCurso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comisiones_Planes",
                schema: "dbo",
                table: "Comisiones");

            migrationBuilder.DropForeignKey(
                name: "FK_personas_Planes_id_plan",
                table: "personas");

            migrationBuilder.DropForeignKey(
                name: "FK_Planes_Especialidades_EspecialidadId",
                table: "Planes");

            migrationBuilder.DropIndex(
                name: "IX_Planes_EspecialidadId",
                table: "Planes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_personas",
                table: "personas");

            migrationBuilder.DropIndex(
                name: "IX_personas_id_plan",
                table: "personas");

            migrationBuilder.DropIndex(
                name: "IX_Comisiones_IdPlan",
                schema: "dbo",
                table: "Comisiones");

            migrationBuilder.RenameTable(
                name: "personas",
                newName: "Personas");

            migrationBuilder.RenameTable(
                name: "Comisiones",
                schema: "dbo",
                newName: "Comisiones");

            migrationBuilder.RenameColumn(
                name: "telefono",
                table: "Personas",
                newName: "Telefono");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Personas",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "legajo",
                table: "Personas",
                newName: "Legajo");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Personas",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "direccion",
                table: "Personas",
                newName: "Direccion");

            migrationBuilder.RenameColumn(
                name: "apellido",
                table: "Personas",
                newName: "Apellido");

            migrationBuilder.RenameColumn(
                name: "tipo_persona",
                table: "Personas",
                newName: "TipoPersona");

            migrationBuilder.RenameColumn(
                name: "id_plan",
                table: "Personas",
                newName: "IdPlan");

            migrationBuilder.RenameColumn(
                name: "fecha_nac",
                table: "Personas",
                newName: "FechaNacimiento");

            migrationBuilder.RenameColumn(
                name: "id_persona",
                table: "Personas",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Personas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Personas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Apellido",
                table: "Personas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Personas",
                table: "Personas",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AlumnoCursos",
                columns: table => new
                {
                    IdInscripcion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAlumno = table.Column<int>(type: "int", nullable: false),
                    IdCurso = table.Column<int>(type: "int", nullable: false),
                    Condicion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nota = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlumnoCursos", x => x.IdInscripcion);
                });

            migrationBuilder.CreateTable(
                name: "Cursos",
                columns: table => new
                {
                    IdCurso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMateria = table.Column<int>(type: "int", nullable: true),
                    IdComision = table.Column<int>(type: "int", nullable: false),
                    AnioCalendario = table.Column<int>(type: "int", nullable: false),
                    Cupo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursos", x => x.IdCurso);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personas_Email",
                table: "Personas",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlumnoCursos_IdAlumno_IdCurso",
                table: "AlumnoCursos",
                columns: new[] { "IdAlumno", "IdCurso" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlumnoCursos");

            migrationBuilder.DropTable(
                name: "Cursos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Personas",
                table: "Personas");

            migrationBuilder.DropIndex(
                name: "IX_Personas_Email",
                table: "Personas");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Personas",
                newName: "personas");

            migrationBuilder.RenameTable(
                name: "Comisiones",
                newName: "Comisiones",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "Telefono",
                table: "personas",
                newName: "telefono");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "personas",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "Legajo",
                table: "personas",
                newName: "legajo");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "personas",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Direccion",
                table: "personas",
                newName: "direccion");

            migrationBuilder.RenameColumn(
                name: "Apellido",
                table: "personas",
                newName: "apellido");

            migrationBuilder.RenameColumn(
                name: "TipoPersona",
                table: "personas",
                newName: "tipo_persona");

            migrationBuilder.RenameColumn(
                name: "IdPlan",
                table: "personas",
                newName: "id_plan");

            migrationBuilder.RenameColumn(
                name: "FechaNacimiento",
                table: "personas",
                newName: "fecha_nac");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "personas",
                newName: "id_persona");

            migrationBuilder.AlterColumn<string>(
                name: "telefono",
                table: "personas",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "personas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "personas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "direccion",
                table: "personas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "apellido",
                table: "personas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_personas",
                table: "personas",
                column: "id_persona");

            migrationBuilder.CreateIndex(
                name: "IX_Planes_EspecialidadId",
                table: "Planes",
                column: "EspecialidadId");

            migrationBuilder.CreateIndex(
                name: "IX_personas_id_plan",
                table: "personas",
                column: "id_plan");

            migrationBuilder.CreateIndex(
                name: "IX_Comisiones_IdPlan",
                schema: "dbo",
                table: "Comisiones",
                column: "IdPlan");

            migrationBuilder.AddForeignKey(
                name: "FK_Comisiones_Planes",
                schema: "dbo",
                table: "Comisiones",
                column: "IdPlan",
                principalTable: "Planes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_personas_Planes_id_plan",
                table: "personas",
                column: "id_plan",
                principalTable: "Planes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Planes_Especialidades_EspecialidadId",
                table: "Planes",
                column: "EspecialidadId",
                principalTable: "Especialidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
