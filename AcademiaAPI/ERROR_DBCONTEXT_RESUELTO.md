# ? ERROR RESUELTO - DbContext Durante OnModelCreating

## ?? Error Original

```
System.InvalidOperationException: An attempt was made to use the model while it was being created. 
A DbContext instance cannot be used inside 'OnModelCreating' in any way that makes use of the model 
that is being created.
```

### ?? Ubicación del Error
**Archivo**: `UsuarioRepository.cs`  
**Método**: `GetAllAsync()`  
**Línea**: `return await _context.Usuarios.Include(...).ToListAsync();`

---

## ?? Causa Raíz

El error ocurría porque el `UsuarioService` se estaba instanciando **antes de que se completara la configuración del modelo de Entity Framework**.

### Código Problemático

```csharp
public static class UsuarioEndpoints
{
    public static void MapUsuariosEndpoints(this WebApplication app) 
    { 
        // ? PROBLEMA: Se crea el servicio ANTES de mapear endpoints
        var usuariosService = new Aplication.Services.UsuarioService();
        
        app.MapGet("/usuarios", () =>
        {
            var usuarios = usuariosService.GetAll();  // Usa la instancia creada arriba
            return Results.Ok(usuarios);
        });
    }
}
```

### ¿Por Qué Causaba el Error?

1. **Al iniciar la aplicación**, se llama a `MapUsuariosEndpoints()`
2. **Se crea** `var usuariosService = new UsuarioService()`
3. **`UsuarioService` crea** `new AcademiaContext()` en su constructor
4. **`AcademiaContext` intenta cargar** el modelo de EF Core
5. **El modelo aún no está completo** porque `OnModelCreating` todavía se está ejecutando
6. **ERROR**: No se puede usar el contexto mientras se está creando

---

## ? Solución Implementada

### Crear el Servicio DENTRO de Cada Endpoint

```csharp
public static class UsuarioEndpoints
{
    public static void MapUsuariosEndpoints(this WebApplication app) 
    { 
        app.MapGet("/usuarios", () =>
        {
            // ? SOLUCIÓN: Crear el servicio dentro del endpoint
            var usuariosService = new Aplication.Services.UsuarioService();
            var usuarios = usuariosService.GetAll();
            return Results.Ok(usuarios);
        });
        
        app.MapGet("/usuarios/{id:int}", (int id) =>
        {
            // ? Nueva instancia para cada request
            var usuariosService = new Aplication.Services.UsuarioService();
            var usuario = usuariosService.GetById(id);
            return usuario == null ? Results.NotFound() : Results.Ok(usuario);
        });
        
        // ...resto de endpoints...
    }
}
```

### ¿Por Qué Funciona Ahora?

1. **Al iniciar la aplicación**, se llama a `MapUsuariosEndpoints()`
2. **Se registran los endpoints** pero NO se ejecutan
3. **El modelo de EF Core se completa** correctamente
4. **Cuando llega un request**, se ejecuta el endpoint
5. **Dentro del endpoint** se crea el servicio (el modelo ya está listo)
6. **? TODO FUNCIONA** correctamente

---

## ?? Comparación Antes/Después

### ? Antes (Problemático)
```
Startup
  ?
MapUsuariosEndpoints()
  ?
new UsuarioService() ? Se crea aquí
  ?
new AcademiaContext() ? Intenta usar el modelo
  ?
OnModelCreating() ? Modelo aún en construcción
  ?
?? ERROR: Modelo siendo usado mientras se crea
```

### ? Después (Correcto)
```
Startup
  ?
MapUsuariosEndpoints()
  ?
Registra endpoints (solo define, no ejecuta)
  ?
OnModelCreating() ? Completa sin problemas
  ?
Modelo Listo ?
  ?
Request llega a /usuarios
  ?
new UsuarioService() ? Se crea ahora
  ?
new AcademiaContext() ? Modelo ya está listo
  ?
? TODO FUNCIONA
```

---

## ?? Ventajas de la Solución

### 1. **Ciclo de Vida Correcto**
- El servicio se crea solo cuando se necesita (por request)
- No hay instancias globales innecesarias

### 2. **Thread-Safe**
- Cada request tiene su propia instancia del servicio
- No hay problemas de concurrencia

### 3. **Mejor Manejo de Recursos**
- El contexto se crea y dispone por request
- No hay conexiones de BD abiertas innecesariamente

### 4. **Fácil de Mantener**
- Patrón claro y consistente en todos los endpoints
- Fácil de entender y modificar

---

## ?? Endpoints Actualizados

Todos los endpoints ahora siguen el mismo patrón:

```csharp
app.MapGet("/usuarios", () =>
{
    var service = new UsuarioService();  // ? Crear instancia
    var result = service.GetAll();       // ? Usar servicio
    return Results.Ok(result);           // ? Retornar resultado
});

app.MapPost("/usuarios", (UsuarioDto dto) =>
{
    var service = new UsuarioService();  // ? Nueva instancia
    service.Add(dto);                    // ? Operación
    return Results.Created(...);         // ? Respuesta
});
```

---

## ?? Alternativa Avanzada (Inyección de Dependencias)

Para proyectos más grandes, se recomienda usar inyección de dependencias:

```csharp
// En Program.cs
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddDbContext<AcademiaContext>();

// En los endpoints
app.MapGet("/usuarios", (IUsuarioService usuarioService) =>
{
    var usuarios = usuarioService.GetAll();
    return Results.Ok(usuarios);
});
```

**Ventajas del DI**:
- ASP.NET Core maneja el ciclo de vida automáticamente
- Mejor para testing (puedes inyectar mocks)
- Más escalable para aplicaciones grandes

**Nota**: La solución actual funciona perfectamente para el tamaño actual del proyecto.

---

## ? Verificación

### Compilación
```
Build successful ?
```

### Prueba del Endpoint
```http
GET http://localhost:5000/usuarios
Response: 200 OK
Content: Lista de usuarios con personas relacionadas
```

---

## ?? Conclusión

El error estaba causado por la **inicialización prematura del servicio** antes de que el modelo de EF Core estuviera completo.

**Solución**: Mover la creación del servicio **dentro de cada endpoint** para que se cree solo cuando se ejecute el request.

**Resultado**: ? Sistema funcionando correctamente

---

**Última actualización**: 25/10/2025 12:03  
**Estado**: ? ERROR RESUELTO
