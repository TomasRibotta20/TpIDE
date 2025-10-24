# Sistema de Migraciones Autom�ticas

## �Qu� hace este sistema?

El proyecto ahora est� configurado para **actualizar autom�ticamente la base de datos** cada vez que lo ejecutas, sin perder datos existentes.

## �C�mo funciona?

1. **Al iniciar el proyecto**: El `MigrationHelper` verifica autom�ticamente si hay cambios pendientes en la estructura de la base de datos.

2. **Si detecta cambios**: Los aplica autom�ticamente usando Entity Framework Core Migrations.

3. **Datos preservados**: Todas tus especialidades, usuarios, planes, comisiones y personas existentes se mantienen intactos.

## �Qu� hacer cuando cambias el modelo de datos?

### Paso 1: Modificar tu modelo
Por ejemplo, si agregas una nueva propiedad a la clase `Persona`:
```csharp
public class Persona
{
    // ...propiedades existentes...
    public string NuevoCampo { get; set; }  // Nueva propiedad
}
```

### Paso 2: Crear una migraci�n
Ejecuta este comando en la terminal (desde la carpeta `Data`):

```powershell
cd ..\Data
dotnet ef migrations add NombreDeTuMigracion --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

**Ejemplos de nombres descriptivos:**
- `AgregaCampoNuevoAPersona`
- `AgregaTablaInscripciones`
- `ModificaRelacionPlanComision`

### Paso 3: Ejecutar el proyecto
�Eso es todo! Cuando ejecutes el proyecto, la migraci�n se aplicar� **autom�ticamente**.

Ver�s en la consola algo como:
```
=== DATABASE MIGRATION CHECK ===
Can connect to database: True
Applied migrations: 1
  ? 20241024181423_InitialCreate
Pending migrations: 1
Applying pending migrations...
  ? 20241024183000_NombreDeTuMigracion
? All migrations applied successfully!
? Database structure is up to date!
```

## Ventajas de este sistema

? **No pierdes datos**: Las migraciones solo modifican la estructura, no eliminan datos.
? **Autom�tico**: No necesitas ejecutar comandos manualmente cada vez.
? **Seguro**: Si algo falla, el error se muestra claramente en la consola.
? **Versionado**: Cada cambio queda registrado en una migraci�n con fecha y nombre.

## Comandos �tiles

### Ver migraciones aplicadas
```powershell
cd ..\Data
dotnet ef migrations list --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

### Revertir la �ltima migraci�n (CUIDADO: puede perder datos)
```powershell
cd ..\Data
dotnet ef migrations remove --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

### Ver el SQL que generar� una migraci�n (sin aplicarla)
```powershell
cd ..\Data
dotnet ef migrations script --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

## Resoluci�n de problemas

### Problema: "Table already exists"
**Soluci�n**: La base de datos fue creada con `EnsureCreated()` antes. 
1. Elimina la base de datos
2. Ejecuta el proyecto nuevamente (se recrear� con migraciones)

### Problema: Error de conexi�n a la base de datos
**Soluci�n**: Verifica que SQL Server est� corriendo y que el `ConnectionString` en `appsettings.json` sea correcto.

### Problema: Conflicto de migraciones
**Soluci�n**: Si dos desarrolladores crean migraciones diferentes:
1. Sincroniza el c�digo con Git
2. Si hay conflicto, elimina tu migraci�n local con `dotnet ef migrations remove`
3. Obt�n las migraciones del repositorio
4. Crea una nueva migraci�n si necesitas m�s cambios

## Estructura de archivos de migraci�n

Cuando creas una migraci�n, se generan archivos en `Data/Migrations/`:
- `[Timestamp]_[Nombre].cs` - C�digo de la migraci�n (m�todos `Up` y `Down`)
- `[Timestamp]_[Nombre].Designer.cs` - Metadatos de la migraci�n
- `AcademiaContextModelSnapshot.cs` - Snapshot del modelo actual (se actualiza autom�ticamente)

**�No borres estos archivos manualmente!** Usa siempre los comandos de EF Core.

## Notas importantes

?? **En producci�n**: Considera aplicar migraciones manualmente antes de desplegar para tener m�s control.

?? **Backups**: Siempre haz backup de la base de datos antes de aplicar migraciones grandes en producci�n.

? **En desarrollo**: El sistema autom�tico es perfecto, te ahorra tiempo y evita errores.
