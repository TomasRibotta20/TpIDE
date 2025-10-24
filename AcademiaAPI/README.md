# ?? Academia API - Sistema Automático con Migraciones

## ?? Inicio Rápido

**Simplemente presiona F5 en Visual Studio.**

Todo está configurado para funcionar automáticamente, incluyendo **migraciones automáticas**.

---

## ? ¿Qué hace automáticamente?

Cuando inicias la API:

1. ?? Verifica la conexión a SQL Server
2. ?? **Aplica migraciones pendientes automáticamente** (NUEVO)
3. ??? Crea la base de datos `Universidad` si no existe
4. ?? Crea/actualiza todas las tablas según tus modelos
5. ?? Crea el usuario admin (`admin` / `admin123`)
6. ?? Inserta datos iniciales (Especialidades)

**No necesitas ejecutar scripts ni configurar nada. ¡Los datos existentes se preservan!**

---

## ?? Primera Vez que Usas el Proyecto

1. Asegúrate de tener **SQL Server** corriendo (localhost,1433)
2. Abre la solución en Visual Studio
3. Configura múltiples proyectos de inicio:
   - Clic derecho en la Solución ? Set Startup Projects
   - Marca: **AcademiaAPI** y **WIndowsForm** como "Start"
4. Presiona **F5**

¡Ya está! La base de datos se crea automáticamente con todas las tablas necesarias.

---

## ?? Modificando el Modelo de Datos (NUEVO SISTEMA)

### Paso 1: Modifica tu código
Ejemplo: agregar una nueva propiedad a `Persona`:
```csharp
public class Persona
{
    // ...propiedades existentes...
    public string NuevoCampo { get; set; }  // Nueva propiedad
}
```

### Paso 2: Crea una migración
Abre una terminal en Visual Studio y ejecuta:
```powershell
cd Data
dotnet ef migrations add NombreDescriptivo --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

Ejemplos de nombres:
- `AgregaCampoTelefonoAlternoAPersona`
- `AgregaTablaInscripciones`
- `ModificaRelacionPlanComision`

### Paso 3: Ejecuta el proyecto
Presiona **F5**. La migración se aplicará **automáticamente**.

Verás en la consola:
```
=== DATABASE MIGRATION CHECK ===
Can connect to database: True
Applied migrations: 1
  ? 20241024211424_InitialCreate
Pending migrations: 1
Applying pending migrations...
  ? 20241024183000_NombreDescriptivo
? All migrations applied successfully!
? Database structure is up to date!
```

### ? Ventajas del Sistema de Migraciones

? **Automático**: Las migraciones se aplican al iniciar el proyecto  
? **Seguro**: Preserva todos los datos existentes (especialidades, usuarios, etc.)  
? **Versionado**: Cada cambio queda registrado con fecha y nombre  
? **Reversible**: Puedes deshacer cambios si es necesario  
? **Colaborativo**: Los cambios se sincronizan vía Git con el equipo  

**?? Para más detalles, lee: [`MIGRACIONES_AUTOMATICAS.md`](MIGRACIONES_AUTOMATICAS.md)**

---

## ?? Conexión a la Base de Datos

Por defecto usa:
```
Server: localhost,1433
Database: Universidad
User: sa
Password: TuContraseñaFuerte123
```

Para cambiarla, edita `appsettings.json` en el proyecto AcademiaAPI.

---

## ?? Credenciales de Prueba

**Usuario:** admin  
**Contraseña:** admin123

Se crea automáticamente al iniciar la API por primera vez.

---

## ?? Endpoints Principales

Swagger está disponible en: `https://localhost:7229/swagger`

Principales endpoints:
- `/auth/login` - Autenticación
- `/personas/alumnos` - Lista de alumnos
- `/personas/profesores` - Lista de profesores
- `/usuarios` - Gestión de usuarios
- `/especialidades` - Especialidades
- `/planes` - Planes de estudio
- `/comisiones` - Comisiones

---

## ?? Estructura del Proyecto

```
TPI(Entrega_2)/
??? AcademiaAPI/          ?? API REST
??? WIndowsForm/          ??? Aplicación Windows Forms
??? Data/                 ?? Acceso a datos (EF Core)
?   ??? Migrations/       ?? Migraciones de la BD (NUEVO)
??? Domain.Model/         ??? Entidades del dominio
??? DTOs/                 ?? Objetos de transferencia
??? Aplication.Services/  ?? Lógica de negocio
??? API.Clients/          ?? Cliente para consumir la API
```

---

## ? Características

? **Code-First** - El código define la estructura de la BD  
? **Migraciones Automáticas** - Los cambios se aplican automáticamente (NUEVO)  
? **Preserva Datos** - Sin pérdida de información  
? **Swagger** - Documentación interactiva de la API  
? **JWT** - Autenticación con tokens  
? **CORS** - Habilitado para desarrollo  

---

## ?? Solución de Problemas

### La API no inicia
- Verifica que SQL Server esté corriendo
- Comprueba las credenciales en `appsettings.json`

### Error en migraciones
- Lee el mensaje en la consola al iniciar
- Verifica que la última migración se haya creado correctamente
- Consulta `MIGRACIONES_AUTOMATICAS.md` para más ayuda

### Error "Table already exists"
**Causa**: La BD fue creada con `EnsureCreated()` antes de las migraciones.  
**Solución**:
1. Elimina la base de datos `Universidad` desde SQL Server Management Studio
2. Ejecuta el proyecto de nuevo (se recreará con migraciones)

### Error de conexión a BD
- Verifica que SQL Server esté en `localhost,1433`
- Comprueba que el usuario `sa` tenga la contraseña correcta
- Asegúrate de que SQL Server acepte conexiones TCP/IP

### No puedo hacer login
- El usuario admin se crea automáticamente
- Usuario: `admin`, Contraseña: `admin123`

---

## ?? Más Información

- **[`MIGRACIONES_AUTOMATICAS.md`](MIGRACIONES_AUTOMATICAS.md)** - Guía completa del sistema de migraciones (NUEVO)
- `Data/README_AUTOMATICO.md` - Detalles del sistema automático
- `Data/LEEME.txt` - Guía visual rápida
- `Data/MIGRACIONES_GUIA.md` - Guía avanzada (opcional)

---

## ?? Tip

**No necesitas hacer nada más que presionar F5.**

El sistema se encarga de todo automáticamente, incluyendo aplicar los cambios en la base de datos. ??

**¿Cambios en el modelo?** ? Crea una migración ? Ejecuta el proyecto ? ¡Listo!
