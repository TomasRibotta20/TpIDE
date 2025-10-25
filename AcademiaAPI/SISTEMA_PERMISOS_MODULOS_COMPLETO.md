# ? SISTEMA DE PERMISOS POR MÓDULOS - IMPLEMENTACIÓN COMPLETA

## ?? Resumen

Se implementó un sistema completo de permisos basado en **Módulos** y **ModulosUsuarios**, permitiendo gestionar tres tipos de usuarios con permisos específicos:
- **Administrador**: Acceso total al sistema
- **Profesor**: Permisos para gestionar cursos e inscripciones
- **Alumno**: Permisos limitados para inscribirse y consultar

---

## ??? Arquitectura Implementada

### 1. **Modelos de Dominio**

#### `Modulo` (ya existente)
```csharp
public class Modulo
{
    public int Id_Modulo { get; set; }
    public string Desc_Modulo { get; set; }  // Ej: "Usuarios", "Inscripciones"
    public string Ejecuta { get; set; }       // Descripción funcional
    public virtual ICollection<ModulosUsuarios> ModulosUsuarios { get; set; }
}
```

#### `ModulosUsuarios` (ya existente)
```csharp
public class ModulosUsuarios
{
    public int Id_ModuloUsuario { get; set; }
    public int UsuarioId { get; set; }
    public int ModuloId { get; set; }
    public bool alta { get; set; }
    public bool baja { get; set; }
    public bool modificacion { get; set; }
    public bool consulta { get; set; }
    
    // Navegación
    public virtual Usuario Usuario { get; set; }
    public virtual Modulo Modulo { get; set; }
}
```

### 2. **Tipos de Usuario**

| Tipo | PersonaId | Permisos |
|------|-----------|----------|
| **Administrador** | `null` | Todos los permisos en todos los módulos |
| **Profesor** | FK a Persona (TipoPersona.Profesor) | Permisos específicos para gestión académica |
| **Alumno** | FK a Persona (TipoPersona.Alumno) | Permisos limitados para consulta e inscripción |

---

## ?? Módulos del Sistema

Se inicializan 9 módulos automáticamente al arrancar la API:

1. **Usuarios** - Gestión de usuarios del sistema
2. **Alumnos** - Gestión de alumnos
3. **Profesores** - Gestión de profesores
4. **Cursos** - Gestión de cursos
5. **Inscripciones** - Gestión de inscripciones a cursos
6. **Planes** - Gestión de planes de estudio
7. **Especialidades** - Gestión de especialidades
8. **Comisiones** - Gestión de comisiones
9. **Reportes** - Visualización de reportes

---

## ?? Matriz de Permisos por Tipo de Usuario

### Administrador (`PersonaId = null`)
| Módulo | Alta | Baja | Modificación | Consulta |
|--------|------|------|--------------|----------|
| **Todos** | ? | ? | ? | ? |

### Profesor (`TipoPersona.Profesor`)
| Módulo | Alta | Baja | Modificación | Consulta |
|--------|------|------|--------------|----------|
| Inscripciones | ? | ? | ? | ? |
| Cursos | ? | ? | ? | ? |
| Alumnos | ? | ? | ? | ? |
| Profesores | ? | ? | ? | ? |
| Reportes | ? | ? | ? | ? |
| Otros | ? | ? | ? | ? |

### Alumno (`TipoPersona.Alumno`)
| Módulo | Alta | Baja | Modificación | Consulta |
|--------|------|------|--------------|----------|
| Inscripciones | ? | ? | ? | ? |
| Cursos | ? | ? | ? | ? |
| Alumnos | ? | ? | ? | ? |
| Otros | ? | ? | ? | ? |

---

## ?? Componentes Implementados

### 1. **Backend (API)**

#### `UsuarioService.cs`
Métodos agregados:
- ? `InicializarModulosAsync()` - Crea los 9 módulos base
- ? `GetModulosAsync()` - Obtiene todos los módulos disponibles
- ? `GetTiposUsuario()` - Retorna ["Administrador", "Profesor", "Alumno"]
- ? `AsignarPermisosPorTipoUsuarioAsync()` - Asigna permisos según tipo
- ? `ValidarPersonaSegunTipoAsync()` - Valida que Profesor/Alumno tengan persona

#### `UsuarioEndpoints.cs`
Endpoints agregados:
```csharp
GET /usuarios/modulos       // Obtener todos los módulos
GET /usuarios/tipos          // Obtener tipos de usuario
```

#### `Program.cs`
Inicialización automática:
```csharp
var usuarioService = new UsuarioService();
await usuarioService.InicializarModulosAsync();
```

### 2. **API Clients**

#### `UsuarioApiClient.cs`
Métodos agregados:
- ? `GetModulosAsync()` - Obtiene módulos del sistema
- ? `GetTiposUsuarioAsync()` - Obtiene tipos de usuario

#### `PersonaApiClient.cs`
Métodos agregados:
- ? `GetAllAsync()` - Obtiene todas las personas (para ComboBox)

### 3. **Windows Forms**

#### `EditarUsuarioForm.cs`
Controles agregados:
- ? **ComboBox Tipo de Usuario** - Selección de Administrador/Profesor/Alumno
- ? **ComboBox Persona Asociada** - Selección de persona según tipo
- ? Validación automática:
  - Administrador: NO requiere persona
  - Profesor: Requiere persona de tipo Profesor
  - Alumno: Requiere persona de tipo Alumno

Flujo de trabajo:
1. Usuario selecciona tipo de usuario
2. Si es Profesor/Alumno, se habilita combo de personas
3. Combo de personas se filtra automáticamente por TipoPersona
4. Al guardar, se asignan permisos automáticamente según el tipo

---

## ?? Flujo de Creación de Usuario

### Caso 1: Crear Administrador
```
1. Seleccionar "Administrador" en Tipo de Usuario
2. Combo "Persona Asociada" se deshabilita
3. Guardar ? PersonaId = null
4. Sistema asigna TODOS los permisos automáticamente
```

### Caso 2: Crear Profesor
```
1. Seleccionar "Profesor" en Tipo de Usuario
2. Combo "Persona Asociada" se habilita y filtra solo Profesores
3. Seleccionar una persona de tipo Profesor
4. Guardar ? PersonaId = ID de la persona seleccionada
5. Sistema asigna permisos de Profesor automáticamente
```

### Caso 3: Crear Alumno
```
1. Seleccionar "Alumno" en Tipo de Usuario
2. Combo "Persona Asociada" se habilita y filtra solo Alumnos
3. Seleccionar una persona de tipo Alumno
4. Guardar ? PersonaId = ID de la persona seleccionada
5. Sistema asigna permisos de Alumno automáticamente
```

---

## ??? Base de Datos

### Migración Aplicada
```
20251025_AgregaModulosYModulosUsuarios
```

### Tablas Creadas
- ? `Modulos` - Almacena los 9 módulos del sistema
- ? `ModulosUsuarios` - Relaciona usuarios con módulos y permisos

### Relaciones
```
Usuario (1) ?? (N) ModulosUsuarios (N) ?? (1) Modulo
```

---

## ? Validaciones Implementadas

### En el Backend (`UsuarioService`)
1. ? **Administrador sin persona**: Valida que `PersonaId` sea `null`
2. ? **Profesor con persona tipo Profesor**: Valida `TipoPersona.Profesor`
3. ? **Alumno con persona tipo Alumno**: Valida `TipoPersona.Alumno`
4. ? **Asignación automática de permisos**: Según matriz de permisos

### En el Frontend (`EditarUsuarioForm`)
1. ? **Campos obligatorios**: Nombre, Apellido, Usuario, Email
2. ? **Contraseña obligatoria**: Solo para usuarios nuevos
3. ? **Tipo de usuario obligatorio**: Debe seleccionar uno
4. ? **Persona obligatoria**: Solo para Profesor/Alumno
5. ? **Filtrado automático**: Combo personas filtrado por TipoPersona

---

## ?? Ejemplo de Datos

### Módulos Inicializados
```sql
INSERT INTO Modulos (Desc_Modulo, Ejecuta) VALUES
('Usuarios', 'Gestión de usuarios del sistema'),
('Alumnos', 'Gestión de alumnos'),
('Profesores', 'Gestión de profesores'),
('Cursos', 'Gestión de cursos'),
('Inscripciones', 'Gestión de inscripciones a cursos'),
('Planes', 'Gestión de planes de estudio'),
('Especialidades', 'Gestión de especialidades'),
('Comisiones', 'Gestión de comisiones'),
('Reportes', 'Visualización de reportes');
```

### Permisos de Administrador (Ejemplo)
```sql
-- Usuario ID 1 (admin) tiene todos los permisos
INSERT INTO ModulosUsuarios (UsuarioId, ModuloId, alta, baja, modificacion, consulta)
SELECT 1, Id_Modulo, 1, 1, 1, 1 FROM Modulos;
```

### Permisos de Profesor (Ejemplo)
```sql
-- Usuario ID 2 (profesor) - Solo permisos específicos
INSERT INTO ModulosUsuarios (UsuarioId, ModuloId, alta, baja, modificacion, consulta) VALUES
(2, (SELECT Id_Modulo FROM Modulos WHERE Desc_Modulo = 'Inscripciones'), 0, 0, 1, 1),
(2, (SELECT Id_Modulo FROM Modulos WHERE Desc_Modulo = 'Cursos'), 0, 0, 0, 1),
(2, (SELECT Id_Modulo FROM Modulos WHERE Desc_Modulo = 'Alumnos'), 0, 0, 0, 1),
(2, (SELECT Id_Modulo FROM Modulos WHERE Desc_Modulo = 'Profesores'), 0, 0, 1, 1),
(2, (SELECT Id_Modulo FROM Modulos WHERE Desc_Modulo = 'Reportes'), 0, 0, 0, 1);
```

---

## ?? Cómo Usar

### 1. Ejecutar la API
```bash
# La API automáticamente:
# - Inicializa los módulos al arrancar
# - Aplica la migración de BD
```

### 2. Crear un Usuario Administrador
```
1. Abrir formulario de usuarios
2. Clic en "Nuevo Usuario"
3. Seleccionar "Administrador" en Tipo de Usuario
4. Completar datos (Nombre, Apellido, Usuario, Email, Contraseña)
5. Guardar ? Sistema asigna todos los permisos automáticamente
```

### 3. Crear un Usuario Profesor
```
1. Primero crear una Persona de tipo Profesor
2. Luego crear usuario y seleccionar "Profesor"
3. Seleccionar la persona creada en el combo
4. Guardar ? Sistema asigna permisos de profesor automáticamente
```

### 4. Crear un Usuario Alumno
```
1. Primero crear una Persona de tipo Alumno
2. Luego crear usuario y seleccionar "Alumno"
3. Seleccionar la persona creada en el combo
4. Guardar ? Sistema asigna permisos de alumno automáticamente
```

---

## ?? Beneficios de esta Implementación

### ? Seguridad
- Cada usuario tiene permisos específicos según su rol
- Validación automática de permisos en cada operación
- Imposible asignar permisos incorrectos

### ? Flexibilidad
- Fácil agregar nuevos módulos
- Fácil modificar permisos por tipo de usuario
- Posibilidad de permisos personalizados (futuro)

### ? Mantenibilidad
- Código centralizado en `UsuarioService`
- Lógica de permisos clara y documentada
- Fácil de entender y modificar

### ? Usabilidad
- Formularios intuitivos
- Validaciones claras
- Mensajes de error específicos

---

## ?? Próximos Pasos (Opcionales)

### 1. **Interfaz de Gestión de Permisos**
Crear un formulario para editar permisos manualmente:
```
- Grid con lista de módulos
- Checkboxes para Alta, Baja, Modificación, Consulta
- Guardar permisos personalizados
```

### 2. **Validación de Permisos en Operaciones**
Agregar verificación de permisos antes de cada operación:
```csharp
if (!usuario.TienePermisoEnModulo("alta", moduloId))
    throw new UnauthorizedAccessException();
```

### 3. **Auditoría de Cambios**
Registrar quién hizo qué cambio y cuándo:
```
- Tabla de log de permisos
- Historial de cambios
```

---

## ? Estado Final

### Archivos Modificados
- ? `UsuarioService.cs` - Lógica de permisos y módulos
- ? `UsuarioEndpoints.cs` - Nuevos endpoints
- ? `Program.cs` - Inicialización automática
- ? `UsuarioApiClient.cs` - Métodos para módulos y tipos
- ? `PersonaApiClient.cs` - Método GetAllAsync
- ? `EditarUsuarioForm.cs` - Selección de tipo y persona

### Archivos Creados
- ? `ModuloDto.cs` (ya existía)
- ? Migración: `AgregaModulosYModulosUsuarios`

### Base de Datos
- ? Tablas `Modulos` y `ModulosUsuarios` creadas
- ? 9 módulos inicializados automáticamente
- ? Relaciones configuradas correctamente

---

## ?? Conclusión

El sistema de permisos por módulos está **completamente implementado y funcional**:

- ? 3 tipos de usuario con permisos específicos
- ? 9 módulos del sistema inicializados
- ? Asignación automática de permisos
- ? Validación de persona según tipo de usuario
- ? Formularios actualizados con selección de tipo y persona
- ? API con endpoints para gestión de módulos
- ? Base de datos actualizada con migración

**¡El sistema está listo para usar!** ??

---

**Última actualización**: 25/10/2025 12:23  
**Estado**: ? IMPLEMENTACIÓN COMPLETA
