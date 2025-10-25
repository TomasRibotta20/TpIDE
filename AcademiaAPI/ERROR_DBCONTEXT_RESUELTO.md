# ? ERROR RESUELTO - DbContext Durante OnModelCreating

## ?? Error Original

```
System.InvalidOperationException: An attempt was made to use the model while it was being created. 
A DbContext instance cannot be used inside 'OnModelCreating' in any way that makes use of the model 
that is being created.
```

### ?? Ubicaci�n del Error
**Archivo**: `UsuarioRepository.cs`  
**M�todo**: `GetAllAsync()`  
**L�nea**: `return await _context.Usuarios.Include(...).ToListAsync();`

---

## ?? Causa Ra�z

El error ocurr�a porque el `UsuarioService` se estaba instanciando **antes de que se completara la configuraci�n del modelo de Entity Framework**.

### C�digo Problem�tico

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

### �Por Qu� Causaba el Error?

1. **Al iniciar la aplicaci�n**, se llama a `MapUsuariosEndpoints()`
2. **Se crea** `var usuariosService = new UsuarioService()`
3. **`UsuarioService` crea** `new AcademiaContext()` en su constructor
4. **`AcademiaContext` intenta cargar** el modelo de EF Core
5. **El modelo a�n no est� completo** porque `OnModelCreating` todav�a se est� ejecutando
6. **ERROR**: No se puede usar el contexto mientras se est� creando

---

## ? Soluci�n Implementada

### Crear el Servicio DENTRO de Cada Endpoint

```csharp
public static class UsuarioEndpoints
{
    public static void MapUsuariosEndpoints(this WebApplication app) 
    { 
        app.MapGet("/usuarios", () =>
        {
            // ? SOLUCI�N: Crear el servicio dentro del endpoint
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

### �Por Qu� Funciona Ahora?

1. **Al iniciar la aplicaci�n**, se llama a `MapUsuariosEndpoints()`
2. **Se registran los endpoints** pero NO se ejecutan
3. **El modelo de EF Core se completa** correctamente
4. **Cuando llega un request**, se ejecuta el endpoint
5. **Dentro del endpoint** se crea el servicio (el modelo ya est� listo)
6. **? TODO FUNCIONA** correctamente

---

## ?? Comparaci�n Antes/Despu�s

### ? Antes (Problem�tico)
```
Startup
  ?
MapUsuariosEndpoints()
  ?
new UsuarioService() ? Se crea aqu�
  ?
new AcademiaContext() ? Intenta usar el modelo
  ?
OnModelCreating() ? Modelo a�n en construcci�n
  ?
?? ERROR: Modelo siendo usado mientras se crea
```

### ? Despu�s (Correcto)
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
new AcademiaContext() ? Modelo ya est� listo
  ?
? TODO FUNCIONA
```

---

## ?? Ventajas de la Soluci�n

### 1. **Ciclo de Vida Correcto**
- El servicio se crea solo cuando se necesita (por request)
- No hay instancias globales innecesarias

### 2. **Thread-Safe**
- Cada request tiene su propia instancia del servicio
- No hay problemas de concurrencia

### 3. **Mejor Manejo de Recursos**
- El contexto se crea y dispone por request
- No hay conexiones de BD abiertas innecesariamente

### 4. **F�cil de Mantener**
- Patr�n claro y consistente en todos los endpoints
- F�cil de entender y modificar

---

## ?? Endpoints Actualizados

Todos los endpoints ahora siguen el mismo patr�n:

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
    service.Add(dto);                    // ? Operaci�n
    return Results.Created(...);         // ? Respuesta
});
```

---

## ?? Alternativa Avanzada (Inyecci�n de Dependencias)

Para proyectos m�s grandes, se recomienda usar inyecci�n de dependencias:

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
- ASP.NET Core maneja el ciclo de vida autom�ticamente
- Mejor para testing (puedes inyectar mocks)
- M�s escalable para aplicaciones grandes

**Nota**: La soluci�n actual funciona perfectamente para el tama�o actual del proyecto.

---

## ? Verificaci�n

### Compilaci�n
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

## ?? Conclusi�n

El error estaba causado por la **inicializaci�n prematura del servicio** antes de que el modelo de EF Core estuviera completo.

**Soluci�n**: Mover la creaci�n del servicio **dentro de cada endpoint** para que se cree solo cuando se ejecute el request.

**Resultado**: ? Sistema funcionando correctamente

---

**�ltima actualizaci�n**: 25/10/2025 12:03  
**Estado**: ? ERROR RESUELTO
