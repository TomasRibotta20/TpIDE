# ?? Sistema de Base de Datos Automático

## ? ¡TODO ES AUTOMÁTICO!

**No necesitas ejecutar ningún script ni comando manual.**

Simplemente **inicia la API (F5)** y el sistema se encargará de todo.

---

## ?? ¿Qué Hace Automáticamente?

Cuando inicias la API, el sistema:

1. ? **Detecta** si la base de datos existe
2. ? **Crea** la base de datos si no existe
3. ? **Aplica** todas las migraciones pendientes
4. ? **Crea** todas las tablas necesarias (Usuarios, Especialidades, Planes, Comisiones, **Personas**)
5. ? **Preserva** todos los datos existentes
6. ? **Crea** el usuario admin si no existe

---

## ?? Flujo de Trabajo

### Para Usuarios Normales (Tú)

```
1. Modificas el código (ej: agregas una propiedad a Persona)
2. Presionas F5 para iniciar la API
3. ¡Listo! La tabla se actualiza automáticamente
```

### Para Desarrolladores (Avanzado - Opcional)

Si quieres tener más control, puedes crear migraciones manualmente:

```bash
cd Data
dotnet ef migrations add NombreDelCambio --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
```

Pero **NO es necesario** - el sistema usa `EnsureCreated()` como fallback si no hay migraciones.

---

## ?? ¿Cómo Funciona?

El sistema usa una estrategia híbrida inteligente:

1. **Si hay migraciones creadas:**
   - Las aplica automáticamente con `Migrate()`
   - Preserva todos los datos

2. **Si NO hay migraciones:**
   - Usa `EnsureCreated()` para crear las tablas
   - También preserva datos si la BD ya existe

---

## ?? Verificación

Al iniciar la API, verás en la consola:

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
? **Automático** - Se ejecuta solo al iniciar  
? **Seguro** - No borra datos existentes  
? **Inteligente** - Detecta qué método usar  
? **Code-First** - El código define la estructura  

---

## ?? Para Nuevos Desarrolladores del Proyecto

Si alguien nuevo clona el repositorio:

1. Configura la conexión a SQL Server en `appsettings.json` (opcional)
2. Presiona F5
3. **¡Listo!** Todo se configura automáticamente

No necesita:
- ? Crear la base de datos manualmente
- ? Ejecutar scripts SQL
- ? Correr migraciones
- ? Configurar nada más

---

## ??? Archivos del Sistema

Los scripts en la carpeta `Data/` son **opcionales** y solo para desarrolladores avanzados:

- `01-crear-migracion-inicial.bat` - Crea migraciones manualmente (opcional)
- `02-nueva-migracion.bat` - Nueva migración manual (opcional)
- `MIGRACIONES_GUIA.md` - Guía avanzada (referencia)

**No necesitas usarlos** - el sistema funciona sin ellos.

---

## ?? Conclusión

**Simplemente presiona F5 y olvídate del resto.**

El sistema se encarga de todo automáticamente. ??
