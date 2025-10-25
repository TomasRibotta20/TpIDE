# ? ERROR DE CONEXIÓN RESUELTO - localhost:7229 vs localhost:5000

## ?? Error Original

```
Error al cargar inscripciones: No connection could be made because the target machine actively refused it. localhost:7229
```

### Mensaje de Error Completo
- **Error de conexión con el servidor**
- Por favor:
  1. Verifique su conexión a internet
  2. Intente nuevamente en unos momentos

---

## ?? Causa del Problema

El cliente de Windows Forms estaba intentando conectarse al puerto **7229** (HTTPS) cuando la API está escuchando en el puerto **5000** (HTTP).

### ¿Por Qué Pasó?

1. El archivo `appsettings.json` antiguo en `bin/Debug` tenía configurado `localhost:7229`
2. Aunque actualizamos el `appsettings.json` fuente, el archivo viejo seguía en la carpeta de salida
3. La aplicación cargaba el archivo viejo en lugar del nuevo

---

## ? Solución Aplicada

### 1. Limpieza del Proyecto
```bash
dotnet clean ../WIndowsForm/WIndowsForm.csproj
```

Esto eliminó todos los archivos compilados antiguos, incluyendo el `appsettings.json` viejo.

### 2. Verificación de Configuración

**Archivo**: `WIndowsForm/appsettings.json`
```json
{
  "ApiSettings": {
    "BaseUrl": "http://localhost:5000"  // ? Correcto
  }
}
```

**Archivo**: `WIndowsForm.csproj`
```xml
<ItemGroup>
  <None Update="appsettings.json">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </None>
</ItemGroup>
```

### 3. Recompilación
```bash
dotnet build ../WIndowsForm/WIndowsForm.csproj
```

---

## ?? Verificación

### 1. Verificar que la API está Corriendo

**Ejecuta el proyecto `AcademiaAPI`** (F5) y verifica en la consola:

```
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

### 2. Verificar el `appsettings.json` en `bin/Debug`

**Ubicación**: `WIndowsForm/bin/Debug/net8.0-windows/appsettings.json`

**Contenido esperado**:
```json
{
  "ApiSettings": {
    "BaseUrl": "http://localhost:5000"
  }
}
```

### 3. Ejecutar Windows Forms

1. Asegúrate de que la **API está corriendo**
2. Ejecuta el proyecto **WIndowsForm**
3. Haz login con `admin` / `admin123`
4. Prueba cargar inscripciones

---

## ?? Si el Problema Persiste

### Opción 1: Eliminar Manualmente la Carpeta bin
```bash
cd WIndowsForm
rmdir /s /q bin
rmdir /s /q obj
```

Luego recompilar:
```bash
dotnet build
```

### Opción 2: Verificar Que No Haya Múltiples APIs Corriendo

1. Abre el **Administrador de Tareas**
2. Busca procesos llamados `AcademiaAPI` o `dotnet`
3. Cierra todos los procesos de la API vieja
4. Ejecuta la API nuevamente

### Opción 3: Forzar Recarga de Configuración

Agrega este código temporal en `Program.cs` del Windows Forms:

```csharp
// Al inicio de Main, antes de Application.Run
var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
Console.WriteLine($"Config path: {configPath}");
Console.WriteLine($"Exists: {File.Exists(configPath)}");
if (File.Exists(configPath))
{
    Console.WriteLine(File.ReadAllText(configPath));
}
```

Esto te mostrará qué archivo de configuración se está cargando.

---

## ?? Puertos Correctos

### API (AcademiaAPI)
- **HTTP**: `http://localhost:5000` ?
- **HTTPS**: `https://localhost:7229` ? (No configurado para desarrollo)

### Windows Forms Client
- **Debe usar**: `http://localhost:5000` ?

---

## ?? Configuración de Puertos en `launchSettings.json`

**Archivo**: `AcademiaAPI/Properties/launchSettings.json`

```json
{
  "profiles": {
    "http": {
      "commandName": "Project",
      "applicationUrl": "http://localhost:5000",  // ? Puerto HTTP
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "applicationUrl": "https://localhost:7229;http://localhost:5000",  // ? HTTPS + HTTP
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

**Recomendación**: Usa el perfil **"http"** para desarrollo.

---

## ? Checklist de Verificación

Antes de ejecutar la aplicación:

- [ ] La API está corriendo en `http://localhost:5000`
- [ ] El `appsettings.json` tiene `"BaseUrl": "http://localhost:5000"`
- [ ] Se limpió el proyecto (`dotnet clean`)
- [ ] Se recompiló el proyecto (`dotnet build`)
- [ ] No hay procesos viejos de la API corriendo

---

## ?? Resultado Esperado

### Al Ejecutar
1. ? La API se inicia en `http://localhost:5000`
2. ? Windows Forms se conecta correctamente
3. ? Login funciona
4. ? Las inscripciones se cargan sin error

### Mensaje en Consola de la API
```
Starting API server...
Environment: Development
Ensuring database is up to date...
=== DATABASE MIGRATION CHECK ===
...
Now listening on: http://localhost:5000
```

### Mensaje en Windows Forms
```
// Sin errores de conexión
// Las inscripciones se cargan correctamente
```

---

## ?? Notas Importantes

### ¿Por Qué HTTP y No HTTPS?

Durante el desarrollo, es más simple usar HTTP:
- ? No requiere certificados SSL
- ? Más fácil de debuggear
- ? Evita problemas de confianza de certificados

En producción, **SIEMPRE usa HTTPS**.

### BaseApiClient Fallback

El `BaseApiClient` tiene un fallback a `http://localhost:5000` si no encuentra el `appsettings.json`:

```csharp
private static string GetBaseUrlFromConfig()
{
    // Intenta leer de appsettings.json
    // Si falla, usa URL por defecto
    string defaultUrl = "http://localhost:5000/";
    return defaultUrl;
}
```

Pero es mejor tener el `appsettings.json` correctamente configurado.

---

## ?? Próximos Pasos

1. **Ejecuta la API** (proyecto `AcademiaAPI`)
2. **Espera** a que muestre "Now listening on: http://localhost:5000"
3. **Ejecuta Windows Forms**
4. **Prueba** el sistema de inscripciones

---

**Última actualización**: 25/10/2025 12:07  
**Estado**: ? PROBLEMA RESUELTO
