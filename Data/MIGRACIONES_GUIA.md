# Gu�a de Migraciones de Entity Framework Core

## ?? Requisitos Previos

Aseg�rate de tener instalada la herramienta de EF Core:

```bash
dotnet tool install --global dotnet-ef
```

O actual�zala si ya la tienes:

```bash
dotnet tool update --global dotnet-ef
```

## ?? Crear una Nueva Migraci�n

Cada vez que agregues o modifiques entidades (como Persona, Alumno, etc.):

### 1. Abre una terminal en la carpeta del proyecto Data:

```bash
cd D:\Documents\Desktop\Tarea\UTN\3er_anio\IDE\TPI(Entrega_2)\Data
```

### 2. Crea la migraci�n:

```bash
dotnet ef migrations add NombreDeLaMigracion --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

Ejemplos de nombres:
- `InitialCreate` - Para la primera migraci�n
- `AddPersonasTable` - Cuando agregaste la tabla Personas
- `AddAlumnosProfesores` - Cuando agregaste los tipos de persona

### 3. Aplicar la migraci�n a la base de datos:

```bash
dotnet ef database update --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

## ?? Aplicaci�n Autom�tica

El c�digo ya est� configurado para aplicar migraciones autom�ticamente cuando inicies la API.

En `Program.cs`:
```csharp
context.Database.Migrate(); // Aplica migraciones pendientes autom�ticamente
```

## ?? Comandos �tiles

### Ver migraciones pendientes:
```bash
dotnet ef migrations list --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

### Eliminar la �ltima migraci�n (si no se aplic� a�n):
```bash
dotnet ef migrations remove --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

### Revertir a una migraci�n espec�fica:
```bash
dotnet ef database update NombreDeLaMigracion --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

### Ver el SQL que generar� una migraci�n:
```bash
dotnet ef migrations script --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

## ?? IMPORTANTE

- **NO borres la carpeta Migrations** - contiene el historial de cambios
- **Siempre crea una nueva migraci�n** antes de cambiar el modelo de datos
- **Las migraciones preservan los datos existentes** - no se borrar�n

## ?? Flujo de Trabajo Recomendado

1. Modificas una entidad en `Domain.Model` (ej: Persona.cs)
2. Creas una migraci�n: `dotnet ef migrations add CambioEnPersona`
3. La migraci�n se aplica autom�ticamente al iniciar la API
4. Los datos existentes se mantienen

## ??? Primera Migraci�n (Ejecutar AHORA)

Para crear la migraci�n inicial con todas las tablas actuales:

```bash
cd D:\Documents\Desktop\Tarea\UTN\3er_anio\IDE\TPI(Entrega_2)\Data
dotnet ef migrations add InitialCreate --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

Esto crear� la carpeta `Migrations` con el snapshot actual de tu base de datos.
