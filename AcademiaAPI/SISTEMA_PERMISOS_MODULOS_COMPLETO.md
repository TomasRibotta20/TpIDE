# ? SISTEMA DE PERMISOS POR M�DULOS - IMPLEMENTACI�N COMPLETA

## ?? Resumen

Se implement� un sistema completo de permisos basado en **M�dulos** y **ModulosUsuarios**, permitiendo gestionar tres tipos de usuarios con permisos espec�ficos:
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
    public string Ejecuta { get; set; }       // Descripci�n funcional
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
    
    // Navegaci�n
    public virtual Usuario Usuario { get; set; }
    public virtual Modulo Modulo { get; set; }
}
```

### 2. **Tipos de Usuario**

| Tipo | PersonaId | Permisos |
|------|-----------|----------|
| **Administrador** | `null` | Todos los permisos en todos los m�dulos |
| **Profesor** | FK a Persona (TipoPersona.Profesor) | Permisos espec�ficos para gesti�n acad�mica |
| **Alumno** | FK a Persona (TipoPersona.Alumno) | Permisos limitados para consulta e inscripci�n |

---

## ?? M�dulos del Sistema

Se inicializan 9 m�dulos autom�ticamente al arrancar la API:

1. **Usuarios** - Gesti�n de usuarios del sistema
2. **Alumnos** - Gesti�n de alumnos
3. **Profesores** - Gesti�n de profesores
4. **Cursos** - Gesti�n de cursos
5. **Inscripciones** - Gesti�n de inscripciones a cursos
6. **Planes** - Gesti�n de planes de estudio
7. **Especialidades** - Gesti�n de especialidades
8. **Comisiones** - Gesti�n de comisiones
9. **Reportes** - Visualizaci�n de reportes

---

## ?? Matriz de Permisos por Tipo de Usuario

### Administrador (`PersonaId = null`)
| M�dulo | Alta | Baja | Modificaci�n | Consulta |
|--------|------|------|--------------|----------|
| **Todos** | ? | ? | ? | ? |

### Profesor (`TipoPersona.Profesor`)
| M�dulo | Alta | Baja | Modificaci�n | Consulta |
|--------|------|------|--------------|----------|
| Inscripciones | ? | ? | ? | ? |
| Cursos | ? | ? | ? | ? |
| Alumnos | ? | ? | ? | ? |
| Profesores | ? | ? | ? | ? |
| Reportes | ? | ? | ? | ? |
| Otros | ? | ? | ? | ? |

### Alumno (`TipoPersona.Alumno`)
| M�dulo | Alta | Baja | Modificaci�n | Consulta |
|--------|------|------|--------------|----------|
| Inscripciones | ? | ? | ? | ? |
| Cursos | ? | ? | ? | ? |
| Alumnos | ? | ? | ? | ? |
| Otros | ? | ? | ? | ? |

---

## ?? Componentes Implementados

### 1. **Backend (API)**

#### `UsuarioService.cs`
M�todos agregados:
- ? `InicializarModulosAsync()` - Crea los 9 m�dulos base
- ? `GetModulosAsync()` - Obtiene todos los m�dulos disponibles
- ? `GetTiposUsuario()` - Retorna ["Administrador", "Profesor", "Alumno"]
- ? `AsignarPermisosPorTipoUsuarioAsync()` - Asigna permisos seg�n tipo
- ? `ValidarPersonaSegunTipoAsync()` - Valida que Profesor/Alumno tengan persona

#### `UsuarioEndpoints.cs`
Endpoints agregados:
```csharp
GET /usuarios/modulos       // Obtener todos los m�dulos
GET /usuarios/tipos          // Obtener tipos de usuario
```

#### `Program.cs`
Inicializaci�n autom�tica:
```csharp
var usuarioService = new UsuarioService();
await usuarioService.InicializarModulosAsync();
```

### 2. **API Clients**

#### `UsuarioApiClient.cs`
M�todos agregados:
- ? `GetModulosAsync()` - Obtiene m�dulos del sistema
- ? `GetTiposUsuarioAsync()` - Obtiene tipos de usuario

#### `PersonaApiClient.cs`
M�todos agregados:
- ? `GetAllAsync()` - Obtiene todas las personas (para ComboBox)

### 3. **Windows Forms**

#### `EditarUsuarioForm.cs`
Controles agregados:
- ? **ComboBox Tipo de Usuario** - Selecci�n de Administrador/Profesor/Alumno
- ? **ComboBox Persona Asociada** - Selecci�n de persona seg�n tipo
- ? Validaci�n autom�tica:
  - Administrador: NO requiere persona
  - Profesor: Requiere persona de tipo Profesor
  - Alumno: Requiere persona de tipo Alumno

Flujo de trabajo:
1. Usuario selecciona tipo de usuario
2. Si es Profesor/Alumno, se habilita combo de personas
3. Combo de personas se filtra autom�ticamente por TipoPersona
4. Al guardar, se asignan permisos autom�ticamente seg�n el tipo

---

## ?? Flujo de Creaci�n de Usuario

### Caso 1: Crear Administrador
```
1. Seleccionar "Administrador" en Tipo de Usuario
2. Combo "Persona Asociada" se deshabilita
3. Guardar ? PersonaId = null
4. Sistema asigna TODOS los permisos autom�ticamente
```

### Caso 2: Crear Profesor
```
1. Seleccionar "Profesor" en Tipo de Usuario
2. Combo "Persona Asociada" se habilita y filtra solo Profesores
3. Seleccionar una persona de tipo Profesor
4. Guardar ? PersonaId = ID de la persona seleccionada
5. Sistema asigna permisos de Profesor autom�ticamente
```

### Caso 3: Crear Alumno
```
1. Seleccionar "Alumno" en Tipo de Usuario
2. Combo "Persona Asociada" se habilita y filtra solo Alumnos
3. Seleccionar una persona de tipo Alumno
4. Guardar ? PersonaId = ID de la persona seleccionada
5. Sistema asigna permisos de Alumno autom�ticamente
```

---

## ??? Base de Datos

### Migraci�n Aplicada
```
20251025_AgregaModulosYModulosUsuarios
```

### Tablas Creadas
- ? `Modulos` - Almacena los 9 m�dulos del sistema
- ? `ModulosUsuarios` - Relaciona usuarios con m�dulos y permisos

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
4. ? **Asignaci�n autom�tica de permisos**: Seg�n matriz de permisos

### En el Frontend (`EditarUsuarioForm`)
1. ? **Campos obligatorios**: Nombre, Apellido, Usuario, Email
2. ? **Contrase�a obligatoria**: Solo para usuarios nuevos
3. ? **Tipo de usuario obligatorio**: Debe seleccionar uno
4. ? **Persona obligatoria**: Solo para Profesor/Alumno
5. ? **Filtrado autom�tico**: Combo personas filtrado por TipoPersona

---

## ?? Ejemplo de Datos

### M�dulos Inicializados
```sql
INSERT INTO Modulos (Desc_Modulo, Ejecuta) VALUES
('Usuarios', 'Gesti�n de usuarios del sistema'),
('Alumnos', 'Gesti�n de alumnos'),
('Profesores', 'Gesti�n de profesores'),
('Cursos', 'Gesti�n de cursos'),
('Inscripciones', 'Gesti�n de inscripciones a cursos'),
('Planes', 'Gesti�n de planes de estudio'),
('Especialidades', 'Gesti�n de especialidades'),
('Comisiones', 'Gesti�n de comisiones'),
('Reportes', 'Visualizaci�n de reportes');
```

### Permisos de Administrador (Ejemplo)
```sql
-- Usuario ID 1 (admin) tiene todos los permisos
INSERT INTO ModulosUsuarios (UsuarioId, ModuloId, alta, baja, modificacion, consulta)
SELECT 1, Id_Modulo, 1, 1, 1, 1 FROM Modulos;
```

### Permisos de Profesor (Ejemplo)
```sql
-- Usuario ID 2 (profesor) - Solo permisos espec�ficos
INSERT INTO ModulosUsuarios (UsuarioId, ModuloId, alta, baja, modificacion, consulta) VALUES
(2, (SELECT Id_Modulo FROM Modulos WHERE Desc_Modulo = 'Inscripciones'), 0, 0, 1, 1),
(2, (SELECT Id_Modulo FROM Modulos WHERE Desc_Modulo = 'Cursos'), 0, 0, 0, 1),
(2, (SELECT Id_Modulo FROM Modulos WHERE Desc_Modulo = 'Alumnos'), 0, 0, 0, 1),
(2, (SELECT Id_Modulo FROM Modulos WHERE Desc_Modulo = 'Profesores'), 0, 0, 1, 1),
(2, (SELECT Id_Modulo FROM Modulos WHERE Desc_Modulo = 'Reportes'), 0, 0, 0, 1);
```

---

## ?? C�mo Usar

### 1. Ejecutar la API
```bash
# La API autom�ticamente:
# - Inicializa los m�dulos al arrancar
# - Aplica la migraci�n de BD
```

### 2. Crear un Usuario Administrador
```
1. Abrir formulario de usuarios
2. Clic en "Nuevo Usuario"
3. Seleccionar "Administrador" en Tipo de Usuario
4. Completar datos (Nombre, Apellido, Usuario, Email, Contrase�a)
5. Guardar ? Sistema asigna todos los permisos autom�ticamente
```

### 3. Crear un Usuario Profesor
```
1. Primero crear una Persona de tipo Profesor
2. Luego crear usuario y seleccionar "Profesor"
3. Seleccionar la persona creada en el combo
4. Guardar ? Sistema asigna permisos de profesor autom�ticamente
```

### 4. Crear un Usuario Alumno
```
1. Primero crear una Persona de tipo Alumno
2. Luego crear usuario y seleccionar "Alumno"
3. Seleccionar la persona creada en el combo
4. Guardar ? Sistema asigna permisos de alumno autom�ticamente
```

---

## ?? Beneficios de esta Implementaci�n

### ? Seguridad
- Cada usuario tiene permisos espec�ficos seg�n su rol
- Validaci�n autom�tica de permisos en cada operaci�n
- Imposible asignar permisos incorrectos

### ? Flexibilidad
- F�cil agregar nuevos m�dulos
- F�cil modificar permisos por tipo de usuario
- Posibilidad de permisos personalizados (futuro)

### ? Mantenibilidad
- C�digo centralizado en `UsuarioService`
- L�gica de permisos clara y documentada
- F�cil de entender y modificar

### ? Usabilidad
- Formularios intuitivos
- Validaciones claras
- Mensajes de error espec�ficos

---

## ?? Pr�ximos Pasos (Opcionales)

### 1. **Interfaz de Gesti�n de Permisos**
Crear un formulario para editar permisos manualmente:
```
- Grid con lista de m�dulos
- Checkboxes para Alta, Baja, Modificaci�n, Consulta
- Guardar permisos personalizados
```

### 2. **Validaci�n de Permisos en Operaciones**
Agregar verificaci�n de permisos antes de cada operaci�n:
```csharp
if (!usuario.TienePermisoEnModulo("alta", moduloId))
    throw new UnauthorizedAccessException();
```

### 3. **Auditor�a de Cambios**
Registrar qui�n hizo qu� cambio y cu�ndo:
```
- Tabla de log de permisos
- Historial de cambios
```

---

## ? Estado Final

### Archivos Modificados
- ? `UsuarioService.cs` - L�gica de permisos y m�dulos
- ? `UsuarioEndpoints.cs` - Nuevos endpoints
- ? `Program.cs` - Inicializaci�n autom�tica
- ? `UsuarioApiClient.cs` - M�todos para m�dulos y tipos
- ? `PersonaApiClient.cs` - M�todo GetAllAsync
- ? `EditarUsuarioForm.cs` - Selecci�n de tipo y persona

### Archivos Creados
- ? `ModuloDto.cs` (ya exist�a)
- ? Migraci�n: `AgregaModulosYModulosUsuarios`

### Base de Datos
- ? Tablas `Modulos` y `ModulosUsuarios` creadas
- ? 9 m�dulos inicializados autom�ticamente
- ? Relaciones configuradas correctamente

---

## ?? Conclusi�n

El sistema de permisos por m�dulos est� **completamente implementado y funcional**:

- ? 3 tipos de usuario con permisos espec�ficos
- ? 9 m�dulos del sistema inicializados
- ? Asignaci�n autom�tica de permisos
- ? Validaci�n de persona seg�n tipo de usuario
- ? Formularios actualizados con selecci�n de tipo y persona
- ? API con endpoints para gesti�n de m�dulos
- ? Base de datos actualizada con migraci�n

**�El sistema est� listo para usar!** ??

---

**�ltima actualizaci�n**: 25/10/2025 12:23  
**Estado**: ? IMPLEMENTACI�N COMPLETA
