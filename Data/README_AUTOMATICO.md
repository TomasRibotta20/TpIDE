# ?? Sistema de Base de Datos Autom�tico

## ? �TODO ES AUTOM�TICO!

**No necesitas ejecutar ning�n script ni comando manual.**

Simplemente **inicia la API (F5)** y el sistema se encargar� de todo.

---

## ?? �Qu� Hace Autom�ticamente?

Cuando inicias la API, el sistema:

1. ? **Detecta** si la base de datos existe
2. ? **Crea** la base de datos si no existe
3. ? **Aplica** todas las migraciones pendientes
4. ? **Crea** todas las tablas necesarias (Usuarios, Especialidades, Planes, Comisiones, **Personas**)
5. ? **Preserva** todos los datos existentes
6. ? **Crea** el usuario admin si no existe

---

## ?? Flujo de Trabajo

### Para Usuarios Normales (T�)

```
1. Modificas el c�digo (ej: agregas una propiedad a Persona)
2. Presionas F5 para iniciar la API
3. �Listo! La tabla se actualiza autom�ticamente
```

### Para Desarrolladores (Avanzado - Opcional)

Si quieres tener m�s control, puedes crear migraciones manualmente:

```bash
cd Data
dotnet ef migrations add NombreDelCambio --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

Pero **NO es necesario** - el sistema usa `EnsureCreated()` como fallback si no hay migraciones.

---

## ?? �C�mo Funciona?

El sistema usa una estrategia h�brida inteligente:

1. **Si hay migraciones creadas:**
   - Las aplica autom�ticamente con `Migrate()`
   - Preserva todos los datos

2. **Si NO hay migraciones:**
   - Usa `EnsureCreated()` para crear las tablas
   - Tambi�n preserva datos si la BD ya existe

---

## ?? Verificaci�n

Al iniciar la API, ver�s en la consola:

```
=== DATABASE MIGRATION CHECK ===
Applied migrations: 0
Pending migrations: 0
No migrations found. Creating database schema...
? Database created successfully!
  ? Usuarios table exists (1 records)
  ? Especialidades table exists (3 records)
  ? Planes table exists (0 records)
  ? Comisiones table exists (0 records)
  ? Personas table exists (0 records)
=== DATABASE READY ===
```

---

## ? Ventajas

? **Zero Configuration** - No necesitas hacer nada  
? **Autom�tico** - Se ejecuta solo al iniciar  
? **Seguro** - No borra datos existentes  
? **Inteligente** - Detecta qu� m�todo usar  
? **Code-First** - El c�digo define la estructura  

---

## ?? Para Nuevos Desarrolladores del Proyecto

Si alguien nuevo clona el repositorio:

1. Configura la conexi�n a SQL Server en `appsettings.json` (opcional)
2. Presiona F5
3. **�Listo!** Todo se configura autom�ticamente

No necesita:
- ? Crear la base de datos manualmente
- ? Ejecutar scripts SQL
- ? Correr migraciones
- ? Configurar nada m�s

---

## ??? Archivos del Sistema

Los scripts en la carpeta `Data/` son **opcionales** y solo para desarrolladores avanzados:

- `01-crear-migracion-inicial.bat` - Crea migraciones manualmente (opcional)
- `02-nueva-migracion.bat` - Nueva migraci�n manual (opcional)
- `MIGRACIONES_GUIA.md` - Gu�a avanzada (referencia)

**No necesitas usarlos** - el sistema funciona sin ellos.

---

## ?? Conclusi�n

**Simplemente presiona F5 y olv�date del resto.**

El sistema se encarga de todo autom�ticamente. ??
