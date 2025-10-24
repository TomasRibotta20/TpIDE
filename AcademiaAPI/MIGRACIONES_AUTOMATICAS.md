# Sistema de Migraciones Automáticas

## ¿Qué hace este sistema?

El proyecto ahora está configurado para **actualizar automáticamente la base de datos** cada vez que lo ejecutas, sin perder datos existentes.

## ¿Cómo funciona?

1. **Al iniciar el proyecto**: El `MigrationHelper` verifica automáticamente si hay cambios pendientes en la estructura de la base de datos.

2. **Si detecta cambios**: Los aplica automáticamente usando Entity Framework Core Migrations.

3. **Datos preservados**: Todas tus especialidades, usuarios, planes, comisiones y personas existentes se mantienen intactos.

## ¿Qué hacer cuando cambias el modelo de datos?

### Paso 1: Modificar tu modelo
Por ejemplo, si agregas una nueva propiedad a la clase `Persona`:
```csharp
public class Persona
{
    // ...propiedades existentes...
    public string NuevoCampo { get; set; }  // Nueva propiedad
}
```

### Paso 2: Crear una migración
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
¡Eso es todo! Cuando ejecutes el proyecto, la migración se aplicará **automáticamente**.

Verás en la consola algo como:
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
? **Automático**: No necesitas ejecutar comandos manualmente cada vez.
? **Seguro**: Si algo falla, el error se muestra claramente en la consola.
? **Versionado**: Cada cambio queda registrado en una migración con fecha y nombre.

## Comandos útiles

### Ver migraciones aplicadas
```powershell
cd ..\Data
dotnet ef migrations list --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

### Revertir la última migración (CUIDADO: puede perder datos)
```powershell
cd ..\Data
dotnet ef migrations remove --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

### Ver el SQL que generará una migración (sin aplicarla)
```powershell
cd ..\Data
dotnet ef migrations script --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

## Resolución de problemas

### Problema: "Table already exists"
**Solución**: La base de datos fue creada con `EnsureCreated()` antes. 
1. Elimina la base de datos
2. Ejecuta el proyecto nuevamente (se recreará con migraciones)

### Problema: Error de conexión a la base de datos
**Solución**: Verifica que SQL Server esté corriendo y que el `ConnectionString` en `appsettings.json` sea correcto.

### Problema: Conflicto de migraciones
**Solución**: Si dos desarrolladores crean migraciones diferentes:
1. Sincroniza el código con Git
2. Si hay conflicto, elimina tu migración local con `dotnet ef migrations remove`
3. Obtén las migraciones del repositorio
4. Crea una nueva migración si necesitas más cambios

## Estructura de archivos de migración

Cuando creas una migración, se generan archivos en `Data/Migrations/`:
- `[Timestamp]_[Nombre].cs` - Código de la migración (métodos `Up` y `Down`)
- `[Timestamp]_[Nombre].Designer.cs` - Metadatos de la migración
- `AcademiaContextModelSnapshot.cs` - Snapshot del modelo actual (se actualiza automáticamente)

**¡No borres estos archivos manualmente!** Usa siempre los comandos de EF Core.

## Notas importantes

?? **En producción**: Considera aplicar migraciones manualmente antes de desplegar para tener más control.

?? **Backups**: Siempre haz backup de la base de datos antes de aplicar migraciones grandes en producción.

? **En desarrollo**: El sistema automático es perfecto, te ahorra tiempo y evita errores.
