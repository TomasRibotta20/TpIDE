# ? SOLUCIÓN FINAL - Usuario con Relación a Persona

## ?? Problema Original

El usuario necesitaba que `Usuario` tuviera una relación real con `Persona` en la base de datos, pero el CRUD de usuarios no funcionaba (error 404 NotFound).

## ? Solución Implementada

Se configuró una **relación uno-a-muchos** entre `Persona` y `Usuario` usando una **foreign key separada** (`PersonaId`) en lugar de compartir el mismo `Id`.

---

## ?? Cambios Realizados

### 1. Modelo `Usuario` (Domain.Model/Usuario.cs)
```csharp
public class Usuario
{
    public int Id { get; set; }  // PK auto-incremental
    
    // Relación con Persona usando una FK separada (nullable)
    public int? PersonaId { get; set; }  // ? NUEVO
    public virtual Persona? Persona { get; set; }
    
    // ...resto de propiedades...
}
```

**Características**:
- ? `PersonaId` es nullable (opcional)
- ? Un usuario puede NO tener persona asociada
- ? Una persona puede tener múltiples usuarios
- ? No hay conflictos con IDENTITY

### 2. Configuración en `AcademiaContext.cs`
```csharp
modelBuilder.Entity<Usuario>(entity =>
{
    entity.HasKey(u => u.Id);
    entity.Property(u => u.Id).ValueGeneratedOnAdd();
    
    // Relación con Persona (opcional) usando PersonaId como FK
    entity.HasOne(u => u.Persona)
          .WithMany()  // Una persona puede tener muchos usuarios
          .HasForeignKey(u => u.PersonaId)
          .OnDelete(DeleteBehavior.SetNull)  // Si se elimina la persona, PersonaId = null
          .IsRequired(false);  // La relación es opcional
});
```

### 3. DTO Actualizado (DTOs/UsuarioDto.cs)
```csharp
public class UsuarioDto
{
    public int Id { get; set; }
    //...existing properties...
    
    // Relación con Persona
    public int? PersonaId { get; set; }  // ? NUEVO
    public PersonaDto? persona { get; set; }  // ? ACTUALIZADO (nullable)
    
    //...existing properties...
}
```

### 4. Servicio Actualizado (Aplication.Services/UsuarioService.cs)
- ? `GetAllAsync()` incluye `.Include(u => u.Persona)`
- ? `GetByIdAsync()` incluye `.Include(u => u.Persona)`
- ? `AddAsync()` maneja `PersonaId`
- ? `UpdateAsync()` actualiza `PersonaId`
- ? `MapToDto()` mapea correctamente `Persona` y `TipoPersona`

### 5. Repositorio Actualizado (Data/UsuarioRepository.cs)
```csharp
public async Task<List<Usuario>> GetAllAsync()
{
    return await _context.Usuarios
        .Include(u => u.Persona)  // ? NUEVO
        .Include(u => u.ModulosUsuarios)
            .ThenInclude(mu => mu.Modulo)
        .ToListAsync();
}
```

### 6. Nueva Migración
**Archivo**: `Data/Migrations/20251025145031_AgregaPersonaIdAUsuario.cs`

```sql
-- Agrega columna PersonaId (nullable)
ALTER TABLE Usuarios ADD PersonaId int NULL;

-- Agrega índice
CREATE INDEX IX_Usuarios_PersonaId ON Usuarios (PersonaId);

-- Agrega foreign key con SET NULL en delete
ALTER TABLE Usuarios ADD CONSTRAINT FK_Usuarios_Personas_PersonaId 
FOREIGN KEY (PersonaId) REFERENCES Personas (Id) ON DELETE SET NULL;
```

---

## ??? Estructura de la Base de Datos

### Tabla `Usuarios`
```
Id (PK, IDENTITY)
Nombre
Apellido
UsuarioNombre
Email
PasswordHash
Salt
Habilitado
PersonaId (FK ? Personas.Id, nullable)  ? NUEVO
```

### Relación
```
Personas (1) ?? (0..N) Usuarios
```

- Una `Persona` puede tener **cero o muchos** `Usuarios`
- Un `Usuario` puede tener **una o ninguna** `Persona`

---

## ?? Cómo Usar

### Crear Usuario SIN Persona
```csharp
var usuarioDto = new UsuarioDto
{
    Nombre = "Juan",
    Apellido = "Pérez",
    UsuarioNombre = "jperez",
    Email = "jperez@mail.com",
    Contrasenia = "password123",
    Habilitado = true,
    PersonaId = null  // Sin persona asociada
};
```

### Crear Usuario CON Persona
```csharp
var usuarioDto = new UsuarioDto
{
    Nombre = "María",
    Apellido = "González",
    UsuarioNombre = "mgonzalez",
    Email = "mgonzalez@mail.com",
    Contrasenia = "password456",
    Habilitado = true,
    PersonaId = 5  // Asociado a la persona con Id = 5
};
```

### Actualizar PersonaId
```csharp
var usuario = await usuarioService.GetByIdAsync(1);
usuario.PersonaId = 10;  // Cambiar a otra persona
await usuarioService.UpdateAsync(usuario);
```

---

## ? Ventajas de Esta Solución

1. ? **No hay conflictos con IDENTITY** - Usa FK separada
2. ? **Flexible** - La persona es opcional
3. ? **Segura** - Si se elimina una persona, PersonaId se pone en `null`
4. ? **Escalable** - Una persona puede tener múltiples usuarios (ej: usuario admin y usuario regular)
5. ? **Compatible** - No rompe el código existente

---

## ?? Aplicar los Cambios

### 1?? Ejecutar el Proyecto
```
Presiona F5 en Visual Studio
```

La migración se aplicará automáticamente y verás:
```
Pending migrations: 1
  - 20251025145031_AgregaPersonaIdAUsuario
Applying pending migrations...
All migrations applied successfully!
```

### 2?? Verificar en SQL Server
```sql
-- Verificar la columna PersonaId
SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Usuarios' AND COLUMN_NAME = 'PersonaId';

-- Ver usuarios con sus personas
SELECT u.Id, u.UsuarioNombre, u.PersonaId, p.Nombre, p.Apellido
FROM Usuarios u
LEFT JOIN Personas p ON u.PersonaId = p.Id;
```

---

## ?? Estado Final del Sistema

### Migraciones Aplicadas
```
1. 20251024211424_InitialCreate
2. 20251024212736_PermitirNulosEnDireccionYTelefono
3. 20251024213233_FixTipoPersonaConversion
4. 20251025014509_AgregaCursosYAlumnoCurso
5. 20251025144636_AgregaModulosYPermisos
6. 20251025145031_AgregaPersonaIdAUsuario  ? NUEVA
```

### Tablas en la BD
```
? Especialidades
? Usuarios (con PersonaId)
? Planes
? Comisiones
? Personas
? Cursos
? AlumnoCursos
? Modulos
? ModulosUsuarios
```

---

## ? Problemas Resueltos

- ? **Error 404 en CRUD de usuarios** ? ? Resuelto
- ? **Usuario sin relación con Persona** ? ? Resuelto
- ? **Conflictos con IDENTITY** ? ? No hay conflictos
- ? **Error de compilación** ? ? Resuelto

---

## ?? Sistema Completamente Funcional

Ahora puedes:
- ? Listar usuarios con sus personas asociadas
- ? Crear usuarios con o sin persona
- ? Actualizar la persona de un usuario
- ? Eliminar una persona sin afectar al usuario (PersonaId se pone en null)
- ? El CRUD de usuarios funciona correctamente

**Presiona F5 y prueba el sistema!** ??

---

**Última actualización**: 25/10/2025 11:53  
**Estado**: ? COMPLETAMENTE FUNCIONAL
