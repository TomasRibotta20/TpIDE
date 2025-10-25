# ? CORRECCIÓN: ASIGNACIÓN AUTOMÁTICA DE PERMISOS

## ?? Problema Identificado

1. **Personas no aparecían en el combo**: El sistema solo buscaba en una tabla "Personas" genérica que no existe
2. **Selección manual de tipo**: El usuario tenía que elegir manualmente el tipo (Administrador/Profesor/Alumno)
3. **Permisos no se asignaban automáticamente**: Se requería configuración manual

## ? Solución Implementada

### 1. **Carga Correcta de Personas**

Ahora el sistema carga **todas las personas** (tanto Profesores como Alumnos) desde la tabla `Personas`:

```csharp
var todasLasPersonas = await _personaApiClient.GetAllAsync();

cmbPersona.DataSource = todasLasPersonas
    .Where(p => p.TipoPersona == TipoPersonaDto.Profesor || 
                p.TipoPersona == TipoPersonaDto.Alumno)
    .Select(p => new
    {
        Persona = p,
        Display = $"{p.Apellido}, {p.Nombre} (Leg: {p.Legajo}) - " +
                  $"{(p.TipoPersona == TipoPersonaDto.Profesor ? "PROFESOR" : "ALUMNO")}"
    })
    .ToList();
```

**Resultado**: El combo muestra:
- `Pérez, Juan (Leg: 123) - PROFESOR`
- `González, María (Leg: 456) - ALUMNO`

### 2. **Detección Automática del Tipo de Usuario**

El sistema **detecta automáticamente** el tipo de usuario según la persona seleccionada:

| Condición | Tipo Detectado | Permisos Asignados |
|-----------|----------------|-------------------|
| Checkbox "Es Administrador" marcado | **Administrador** | Todos los permisos |
| Persona con `TipoPersona.Profesor` | **Profesor** | Permisos de profesor |
| Persona con `TipoPersona.Alumno` | **Alumno** | Permisos de alumno |

### 3. **Interfaz Mejorada**

#### Controles del Formulario:
1. **CheckBox "Es Administrador"**
   - Si se marca: No requiere persona, asigna permisos de administrador
   - Si no se marca: Requiere persona, tipo se detecta automáticamente

2. **ComboBox "Persona Asociada"**
   - Muestra profesores y alumnos
   - Se deshabilita si "Es Administrador" está marcado
   - Formato: `Apellido, Nombre (Legajo) - TIPO`

3. **Label "Tipo Detectado"** (nuevo)
   - Muestra en azul el tipo detectado automáticamente
   - Ejemplos:
     - `Tipo: ADMINISTRADOR (acceso total al sistema)`
     - `Tipo: PROFESOR (puede gestionar cursos e inscripciones)`
     - `Tipo: ALUMNO (puede inscribirse a cursos)`

---

## ?? Flujo de Trabajo

### Caso 1: Crear Administrador
```
1. Marcar checkbox "Es Administrador"
2. Combo "Persona Asociada" se deshabilita automáticamente
3. Label muestra: "Tipo: ADMINISTRADOR (acceso total al sistema)"
4. Completar datos básicos
5. Guardar ? PersonaId = null ? Sistema asigna TODOS los permisos
```

### Caso 2: Crear Profesor
```
1. NO marcar checkbox "Es Administrador"
2. Seleccionar una persona de tipo PROFESOR en el combo
3. Label muestra automáticamente: "Tipo: PROFESOR (puede gestionar cursos e inscripciones)"
4. Completar datos básicos
5. Guardar ? PersonaId = ID del profesor ? Sistema asigna permisos de profesor
```

### Caso 3: Crear Alumno
```
1. NO marcar checkbox "Es Administrador"
2. Seleccionar una persona de tipo ALUMNO en el combo
3. Label muestra automáticamente: "Tipo: ALUMNO (puede inscribirse a cursos)"
4. Completar datos básicos
5. Guardar ? PersonaId = ID del alumno ? Sistema asigna permisos de alumno
```

---

## ?? Validaciones Implementadas

### En el Formulario
1. ? **Campos obligatorios**: Nombre, Apellido, Usuario, Email
2. ? **Contraseña**: Obligatoria solo para usuarios nuevos
3. ? **Persona o Administrador**: Debe seleccionar persona O marcar administrador
4. ? **Detección automática**: El tipo se detecta según la persona seleccionada

### En el Backend (UsuarioService)
1. ? **PersonaId = null**: Asigna permisos de Administrador
2. ? **TipoPersona.Profesor**: Asigna permisos de Profesor
3. ? **TipoPersona.Alumno**: Asigna permisos de Alumno

---

## ?? Cambios en la Interfaz

### Antes
```
[Tipo de Usuario*: ?]  ? Selección manual
[Persona Asociada: ?]  ? Podía estar vacío
```

### Después
```
[? Es Administrador (sin persona asociada)]  ? Checkbox intuitivo
[Persona Asociada*: ?]                        ? Se deshabilita si es admin
[Tipo: PROFESOR (puede gestionar cursos...)]  ? Detección automática
```

---

## ? Beneficios

### 1. **Simplicidad**
- Usuario no necesita entender tipos de usuario
- Solo selecciona una persona y el sistema detecta automáticamente

### 2. **Seguridad**
- Imposible asignar permisos incorrectos
- El tipo se determina por el TipoPersona registrado

### 3. **Claridad**
- Muestra claramente qué tipo de usuario se está creando
- Indica los permisos que tendrá

### 4. **Flexibilidad**
- Administradores sin persona asociada
- Profesores y Alumnos vinculados a personas reales

---

## ??? Datos en la Base de Datos

### Tabla Personas (existente)
```sql
SELECT Id, Nombre, Apellido, Legajo, TipoPersona
FROM Personas;

-- Ejemplo:
-- 1, Juan, Pérez, 123, 1  (Profesor)
-- 2, María, González, 456, 0  (Alumno)
```

### Tabla Usuarios (después de crear)
```sql
SELECT Id, UsuarioNombre, PersonaId
FROM Usuarios;

-- Ejemplo:
-- 1, admin, NULL          (Administrador)
-- 2, jperez, 1            (Profesor - vinculado a Juan Pérez)
-- 3, mgonzalez, 2         (Alumno - vinculado a María González)
```

### Tabla ModulosUsuarios (permisos automáticos)
```sql
SELECT UsuarioId, ModuloId, alta, baja, modificacion, consulta
FROM ModulosUsuarios
WHERE UsuarioId = 2;  -- Profesor jperez

-- Resultado: Permisos de profesor asignados automáticamente
-- (consulta en Cursos, Alumnos, Reportes)
-- (modificación en Inscripciones, Profesores)
```

---

## ?? Cómo Probar

### 1. Crear Personas Primero
```
1. Ir a "Gestión de Alumnos" ? Crear un alumno (Ej: María González)
2. Ir a "Gestión de Profesores" ? Crear un profesor (Ej: Juan Pérez)
```

### 2. Crear Usuario Alumno
```
1. Ir a "Gestión de Usuarios" ? "Nuevo Usuario"
2. NO marcar "Es Administrador"
3. Seleccionar en el combo: "González, María (Leg: 456) - ALUMNO"
4. Ver que aparece: "Tipo: ALUMNO (puede inscribirse a cursos)"
5. Completar datos y guardar
```

### 3. Crear Usuario Profesor
```
1. Ir a "Gestión de Usuarios" ? "Nuevo Usuario"
2. NO marcar "Es Administrador"
3. Seleccionar en el combo: "Pérez, Juan (Leg: 123) - PROFESOR"
4. Ver que aparece: "Tipo: PROFESOR (puede gestionar cursos e inscripciones)"
5. Completar datos y guardar
```

### 4. Crear Usuario Administrador
```
1. Ir a "Gestión de Usuarios" ? "Nuevo Usuario"
2. MARCAR "Es Administrador"
3. Ver que el combo de personas se deshabilita
4. Ver que aparece: "Tipo: ADMINISTRADOR (acceso total al sistema)"
5. Completar datos y guardar
```

---

## ?? Matriz de Permisos (Sin Cambios)

Los permisos se asignan automáticamente según esta matriz:

### Administrador
| Módulo | Alta | Baja | Modificación | Consulta |
|--------|------|------|--------------|----------|
| Todos | ? | ? | ? | ? |

### Profesor
| Módulo | Alta | Baja | Modificación | Consulta |
|--------|------|------|--------------|----------|
| Inscripciones | ? | ? | ? | ? |
| Cursos, Alumnos, Reportes | ? | ? | ? | ? |
| Profesores | ? | ? | ? | ? |

### Alumno
| Módulo | Alta | Baja | Modificación | Consulta |
|--------|------|------|--------------|----------|
| Inscripciones | ? | ? | ? | ? |
| Cursos, Alumnos | ? | ? | ? | ? |

---

## ? Resumen de Cambios

### Archivos Modificados
- ? `EditarUsuarioForm.cs` - Detección automática del tipo de usuario

### Cambios Clave
1. ? Eliminada selección manual de "Tipo de Usuario"
2. ? Agregado CheckBox "Es Administrador"
3. ? Combo de personas muestra TODOS (profesores y alumnos)
4. ? Label que muestra el tipo detectado automáticamente
5. ? Validaciones mejoradas

### Backend (Sin Cambios)
- ? `UsuarioService.cs` ya asigna permisos automáticamente según PersonaId
- ? Lógica de detección de tipo ya implementada

---

## ?? Resultado Final

El sistema ahora:
- ? **Carga correctamente** todas las personas (profesores y alumnos)
- ? **Detecta automáticamente** el tipo de usuario según la persona
- ? **Asigna automáticamente** los permisos correspondientes
- ? **Muestra claramente** qué tipo de usuario se está creando
- ? **Valida correctamente** que se seleccione persona o se marque administrador

**¡Sistema listo para usar!** ??

---

**Última actualización**: 25/10/2025 12:26  
**Estado**: ? FUNCIONANDO CORRECTAMENTE
