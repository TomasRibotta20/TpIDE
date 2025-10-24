# ?? Academia API - Sistema Autom�tico con Migraciones

## ?? Inicio R�pido

**Simplemente presiona F5 en Visual Studio.**

Todo est� configurado para funcionar autom�ticamente, incluyendo **migraciones autom�ticas**.

---

## ? �Qu� hace autom�ticamente?

Cuando inicias la API:

1. ?? Verifica la conexi�n a SQL Server
2. ?? **Aplica migraciones pendientes autom�ticamente** (NUEVO)
3. ??? Crea la base de datos `Universidad` si no existe
4. ?? Crea/actualiza todas las tablas seg�n tus modelos
5. ?? Crea el usuario admin (`admin` / `admin123`)
6. ?? Inserta datos iniciales (Especialidades)

**No necesitas ejecutar scripts ni configurar nada. �Los datos existentes se preservan!**

---

## ?? Primera Vez que Usas el Proyecto

1. Aseg�rate de tener **SQL Server** corriendo (localhost,1433)
2. Abre la soluci�n en Visual Studio
3. Configura m�ltiples proyectos de inicio:
   - Clic derecho en la Soluci�n ? Set Startup Projects
   - Marca: **AcademiaAPI** y **WIndowsForm** como "Start"
4. Presiona **F5**

�Ya est�! La base de datos se crea autom�ticamente con todas las tablas necesarias.

---

## ?? Modificando el Modelo de Datos (NUEVO SISTEMA)

### Paso 1: Modifica tu c�digo
Ejemplo: agregar una nueva propiedad a `Persona`:
```csharp
public class Persona
{
    // ...propiedades existentes...
    public string NuevoCampo { get; set; }  // Nueva propiedad
}
```

### Paso 2: Crea una migraci�n
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
Presiona **F5**. La migraci�n se aplicar� **autom�ticamente**.

Ver�s en la consola:
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

? **Autom�tico**: Las migraciones se aplican al iniciar el proyecto  
? **Seguro**: Preserva todos los datos existentes (especialidades, usuarios, etc.)  
? **Versionado**: Cada cambio queda registrado con fecha y nombre  
? **Reversible**: Puedes deshacer cambios si es necesario  
? **Colaborativo**: Los cambios se sincronizan v�a Git con el equipo  

**?? Para m�s detalles, lee: [`MIGRACIONES_AUTOMATICAS.md`](MIGRACIONES_AUTOMATICAS.md)**

---

## ?? Conexi�n a la Base de Datos

Por defecto usa:
```
Server: localhost,1433
Database: Universidad
User: sa
Password: TuContrase�aFuerte123
```

Para cambiarla, edita `appsettings.json` en el proyecto AcademiaAPI.

---

## ?? Credenciales de Prueba

**Usuario:** admin  
**Contrase�a:** admin123

Se crea autom�ticamente al iniciar la API por primera vez.

---

## ?? Endpoints Principales

Swagger est� disponible en: `https://localhost:7229/swagger`

Principales endpoints:
- `/auth/login` - Autenticaci�n
- `/personas/alumnos` - Lista de alumnos
- `/personas/profesores` - Lista de profesores
- `/usuarios` - Gesti�n de usuarios
- `/especialidades` - Especialidades
- `/planes` - Planes de estudio
- `/comisiones` - Comisiones

---

## ?? Estructura del Proyecto

```
TPI(Entrega_2)/
??? AcademiaAPI/          ?? API REST
??? WIndowsForm/          ??? Aplicaci�n Windows Forms
??? Data/                 ?? Acceso a datos (EF Core)
?   ??? Migrations/       ?? Migraciones de la BD (NUEVO)
??? Domain.Model/         ??? Entidades del dominio
??? DTOs/                 ?? Objetos de transferencia
??? Aplication.Services/  ?? L�gica de negocio
??? API.Clients/          ?? Cliente para consumir la API
```

---

## ? Caracter�sticas

? **Code-First** - El c�digo define la estructura de la BD  
? **Migraciones Autom�ticas** - Los cambios se aplican autom�ticamente (NUEVO)  
? **Preserva Datos** - Sin p�rdida de informaci�n  
? **Swagger** - Documentaci�n interactiva de la API  
? **JWT** - Autenticaci�n con tokens  
? **CORS** - Habilitado para desarrollo  

---

## ?? Soluci�n de Problemas

### La API no inicia
- Verifica que SQL Server est� corriendo
- Comprueba las credenciales en `appsettings.json`

### Error en migraciones
- Lee el mensaje en la consola al iniciar
- Verifica que la �ltima migraci�n se haya creado correctamente
- Consulta `MIGRACIONES_AUTOMATICAS.md` para m�s ayuda

### Error "Table already exists"
**Causa**: La BD fue creada con `EnsureCreated()` antes de las migraciones.  
**Soluci�n**:
1. Elimina la base de datos `Universidad` desde SQL Server Management Studio
2. Ejecuta el proyecto de nuevo (se recrear� con migraciones)

### Error de conexi�n a BD
- Verifica que SQL Server est� en `localhost,1433`
- Comprueba que el usuario `sa` tenga la contrase�a correcta
- Aseg�rate de que SQL Server acepte conexiones TCP/IP

### No puedo hacer login
- El usuario admin se crea autom�ticamente
- Usuario: `admin`, Contrase�a: `admin123`

---

## ?? M�s Informaci�n

- **[`MIGRACIONES_AUTOMATICAS.md`](MIGRACIONES_AUTOMATICAS.md)** - Gu�a completa del sistema de migraciones (NUEVO)
- `Data/README_AUTOMATICO.md` - Detalles del sistema autom�tico
- `Data/LEEME.txt` - Gu�a visual r�pida
- `Data/MIGRACIONES_GUIA.md` - Gu�a avanzada (opcional)

---

## ?? Tip

**No necesitas hacer nada m�s que presionar F5.**

El sistema se encarga de todo autom�ticamente, incluyendo aplicar los cambios en la base de datos. ??

**�Cambios en el modelo?** ? Crea una migraci�n ? Ejecuta el proyecto ? �Listo!
