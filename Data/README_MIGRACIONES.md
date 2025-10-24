# ?? Configuraci�n de Migraciones - Pasos a Seguir

## ? Paso 1: Instalar herramientas de EF Core

Abre una terminal de PowerShell o CMD y ejecuta:

```bash
dotnet tool install --global dotnet-ef
```

Si ya la tienes instalada, actual�zala:

```bash
dotnet tool update --global dotnet-ef
```

## ? Paso 2: Crear la Migraci�n Inicial

Abre una terminal en la carpeta del proyecto **Data**:

```bash
cd "D:\Documents\Desktop\Tarea\UTN\3er_anio\IDE\TPI(Entrega_2)\Data"
```

Crea la migraci�n inicial:

```bash
dotnet ef migrations add InitialCreate --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

Esto crear�:
- ?? Carpeta `Migrations` en el proyecto Data
- ?? Archivos de migraci�n con timestamp
- ?? `AcademiaContextModelSnapshot.cs` (snapshot del modelo)

## ? Paso 3: Aplicar la Migraci�n

Ejecuta:

```bash
dotnet ef database update --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

O simplemente **inicia la API** - las migraciones se aplicar�n autom�ticamente.

## ? Paso 4: Verificar

1. Inicia la API (F5 en Visual Studio)
2. Deber�as ver en la consola:
   ```
   Applying database migrations...
   Database migrations applied successfully.
   ```
3. Abre SQL Server Management Studio
4. Verifica que existe la tabla `personas`

## ?? De Ahora en Adelante

### Cada vez que agregues o modifiques una entidad:

1. **Crea una migraci�n:**
   ```bash
   cd "D:\Documents\Desktop\Tarea\UTN\3er_anio\IDE\TPI(Entrega_2)\Data"
   dotnet ef migrations add DescripcionDelCambio --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
   ```

2. **Inicia la API** - La migraci�n se aplicar� autom�ticamente

### Ejemplo: Si agregas una nueva entidad "Profesor"

```bash
dotnet ef migrations add AddProfesorTable --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

## ? Uso del Script PowerShell (Opcional)

Para facilitar el proceso, usa el script:

```powershell
cd "D:\Documents\Desktop\Tarea\UTN\3er_anio\IDE\TPI(Entrega_2)\Data"
.\create-migration.ps1 "NombreDeLaMigracion"
```

## ? Soluci�n de Problemas

### Error: "Build failed"
```bash
# Aseg�rate de compilar primero
dotnet build ..\AcademiaAPI\AcademiaAPI.csproj
```

### Error: "No executable found matching command dotnet-ef"
```bash
# Reinstala las herramientas
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef
```

### Error: "The migration has already been applied"
- No hay problema, simplemente ya estaba aplicada
- Las migraciones solo se aplican una vez

## ?? Ventajas de Este Enfoque

? **Code-First**: El c�digo define la estructura de la BD  
? **Preserva Datos**: Las migraciones no borran datos existentes  
? **Versionado**: Cada cambio queda registrado  
? **Autom�tico**: Se aplica al iniciar la API  
? **Reversible**: Puedes volver a versiones anteriores  

## ?? Verificar Estado de Migraciones

Ver migraciones aplicadas:
```bash
dotnet ef migrations list --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

Ver SQL de la pr�xima migraci�n:
```bash
dotnet ef migrations script --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

---

**�Listo!** Ahora tienes un sistema Code-First completo que preserva tus datos. ??
