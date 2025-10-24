# ? CHECKLIST DE VERIFICACIÓN

## Antes de Ejecutar el Proyecto

### 1. SQL Server
- [ ] SQL Server está corriendo en `localhost,1433`
- [ ] El usuario `sa` tiene la contraseña `TuContraseñaFuerte123`
- [ ] (O has actualizado `appsettings.json` con tus credenciales)

### 2. Primera Ejecución (Si ya tenías BD anterior)
- [ ] Eliminar la base de datos `Universidad` desde SSMS (si existe)
  - Clic derecho en `Universidad` ? Delete
  - Marcar "Close existing connections"
- [ ] O verificar que fue creada con `EnsureCreated()` para eliminarla

## Al Ejecutar el Proyecto (F5)

### 3. Consola de la API - Deberías Ver:
```
Starting API server...
Environment: Development
=== DATABASE MIGRATION CHECK ===
Can connect to database: True
Applied migrations: 1
  ? 20251024211424_InitialCreate
Pending migrations: 0
? Database is already up to date!

Verifying database tables:
  ? Usuarios table exists (X records)
  ? Especialidades table exists (3 records)
  ? Planes table exists (X records)
  ? Comisiones table exists (X records)
  ? Personas table exists (X records)  ? ¡IMPORTANTE!
=== DATABASE READY ===
Ensuring admin user exists...
```

### 4. Verificar en SQL Server Management Studio

#### Abrir SSMS y conectar a localhost
- [ ] Expandir `Databases`
- [ ] Ver base de datos `Universidad`
- [ ] Expandir `Tables`

#### Deberías ver estas tablas:
- [ ] `dbo.Comisiones`
- [ ] `dbo.Especialidades`
- [ ] `dbo.Planes`
- [ ] `dbo.Usuarios`
- [ ] `dbo.personas` ? **¡Esta es la tabla de alumnos!**
- [ ] `dbo.__EFMigrationsHistory` (tabla de control de EF)

#### Verificar datos iniciales:
```sql
-- Especialidades (deben existir 3)
SELECT * FROM Especialidades
-- Resultado esperado: Artes, Humanidades, Tecnico Electronico

-- Usuario admin (debe existir 1)
SELECT * FROM Usuarios WHERE UsuarioNombre = 'admin'
-- Resultado esperado: 1 registro

-- Personas (puede estar vacía inicialmente)
SELECT * FROM personas
```

### 5. Probar la API

#### En Swagger (https://localhost:7229/swagger)
- [ ] Endpoint `/auth/login` existe
- [ ] Login con `admin` / `admin123` funciona
- [ ] Endpoint `/personas/alumnos` existe
- [ ] Endpoint `/personas/profesores` existe
- [ ] GET a `/personas/alumnos` devuelve 200 (aunque esté vacío)

#### En el Cliente Windows Forms
- [ ] Login con `admin` / `admin123` funciona
- [ ] Menú principal se abre correctamente
- [ ] Al hacer clic en "Alumnos" ? "Listar Alumnos":
  - [ ] No hay error 500
  - [ ] Se muestra el formulario (aunque esté vacío)
  - [ ] ? **PROBLEMA RESUELTO**: Ya no aparece el error de tabla inexistente

### 6. Probar Creación de Alumno
- [ ] En Windows Forms, ir a "Alumnos" ? "Nuevo Alumno"
- [ ] Completar el formulario
- [ ] Guardar
- [ ] Verificar que aparece en la lista
- [ ] Verificar en SQL Server:
  ```sql
  SELECT * FROM personas WHERE tipo_persona = 'Alumno'
  ```

## Próximos Pasos (Opcional)

### Probar el Sistema de Migraciones

#### 1. Hacer un cambio en el modelo
Editar `Domain.Model/Persona.cs`:
```csharp
public class Persona
{
    // ...propiedades existentes...
    public string TelefonoAlternativo { get; set; }  // NUEVO CAMPO
    // ...resto del código...
}
```

#### 2. Crear la migración
```powershell
cd Data
dotnet ef migrations add AgregaTelefonoAlternativoAPersona --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

#### 3. Ejecutar el proyecto (F5)
- [ ] En la consola debe aparecer:
  ```
  Pending migrations: 1
  Applying pending migrations...
    ? 20241024XXXXXX_AgregaTelefonoAlternativoAPersona
  ? All migrations applied successfully!
  ```

#### 4. Verificar en SSMS
```sql
-- Debe aparecer la nueva columna
SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'personas' AND COLUMN_NAME = 'TelefonoAlternativo'
```

#### 5. Verificar que los datos existentes se preservaron
```sql
SELECT * FROM personas
-- Los registros anteriores deben seguir existiendo
```

## ? Si Algo Falla

### Error: "Table already exists"
**Causa**: Base de datos creada con `EnsureCreated()`  
**Solución**: Eliminar BD y ejecutar de nuevo

### Error: "Cannot connect to database"
**Solución**: 
1. Verificar que SQL Server esté corriendo
2. Verificar credenciales en `appsettings.json`
3. Verificar que SQL Server acepte conexiones TCP/IP

### Error: "A network-related error occurred"
**Solución**:
1. Abrir SQL Server Configuration Manager
2. Ir a SQL Server Network Configuration ? Protocols
3. Habilitar TCP/IP
4. Reiniciar servicio SQL Server

### Error al listar alumnos pero la tabla existe
**Posibles causas**:
1. La API no está corriendo
2. Error en el endpoint (verificar en Swagger)
3. Error de red entre Windows Forms y API

## ? Todo Está Listo Cuando...

- [x] La compilación es exitosa
- [ ] SQL Server está corriendo
- [ ] Al ejecutar F5, no hay errores en consola
- [ ] Las 5 tablas existen en la BD
- [ ] Login funciona en Windows Forms
- [ ] Listado de alumnos no da error 500
- [ ] (Opcional) Puedes crear un alumno de prueba

## ?? ¡Sistema Funcionando Correctamente!

Si todos los checks pasan, el sistema está funcionando perfectamente.

**La tabla de alumnos se crea automáticamente y los datos se preservan al hacer cambios.**

---

## ?? Soporte

Si tienes problemas, revisa:
1. `SOLUCION_IMPLEMENTADA.md` - Resumen de cambios
2. `MIGRACIONES_AUTOMATICAS.md` - Guía completa
3. `README.md` - Documentación general

**Comandos útiles:**
```powershell
# Ver migraciones aplicadas
cd Data
dotnet ef migrations list --startup-project ..\AcademiaAPI\AcademiaAPI.csproj

# Ver SQL de migración
dotnet ef migrations script --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```
