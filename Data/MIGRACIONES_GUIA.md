# Guía de Migraciones de Entity Framework Core

## ?? Requisitos Previos

Asegúrate de tener instalada la herramienta de EF Core:

```bash
dotnet tool install --global dotnet-ef
```

O actualízala si ya la tienes:

```bash
dotnet tool update --global dotnet-ef
```

## ?? Crear una Nueva Migración

Cada vez que agregues o modifiques entidades (como Persona, Alumno, etc.):

### 1. Abre una terminal en la carpeta del proyecto Data:

```bash
cd D:\Documents\Desktop\Tarea\UTN\3er_anio\IDE\TPI(Entrega_2)\Data
```

### 2. Crea la migración:

```bash
dotnet ef migrations add NombreDeLaMigracion --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

Ejemplos de nombres:
- `InitialCreate` - Para la primera migración
- `AddPersonasTable` - Cuando agregaste la tabla Personas
- `AddAlumnosProfesores` - Cuando agregaste los tipos de persona

### 3. Aplicar la migración a la base de datos:

```bash
dotnet ef database update --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

## ?? Aplicación Automática

El código ya está configurado para aplicar migraciones automáticamente cuando inicies la API.

En `Program.cs`:
```csharp
context.Database.Migrate(); // Aplica migraciones pendientes automáticamente
```

## ?? Comandos Útiles

### Ver migraciones pendientes:
```bash
dotnet ef migrations list --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

### Eliminar la última migración (si no se aplicó aún):
```bash
dotnet ef migrations remove --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

### Revertir a una migración específica:
```bash
dotnet ef database update NombreDeLaMigracion --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

### Ver el SQL que generará una migración:
```bash
dotnet ef migrations script --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

## ?? IMPORTANTE

- **NO borres la carpeta Migrations** - contiene el historial de cambios
- **Siempre crea una nueva migración** antes de cambiar el modelo de datos
- **Las migraciones preservan los datos existentes** - no se borrarán

## ?? Flujo de Trabajo Recomendado

1. Modificas una entidad en `Domain.Model` (ej: Persona.cs)
2. Creas una migración: `dotnet ef migrations add CambioEnPersona`
3. La migración se aplica automáticamente al iniciar la API
4. Los datos existentes se mantienen

## ??? Primera Migración (Ejecutar AHORA)

Para crear la migración inicial con todas las tablas actuales:

```bash
cd D:\Documents\Desktop\Tarea\UTN\3er_anio\IDE\TPI(Entrega_2)\Data
dotnet ef migrations add InitialCreate --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

Esto creará la carpeta `Migrations` con el snapshot actual de tu base de datos.
