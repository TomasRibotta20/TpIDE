# SISTEMA DE MENUS DIFERENCIADOS - IMPLEMENTACION PARCIAL

## ESTADO ACTUAL

El sistema tiene implementado:
- ? Detección automática del tipo de usuario en el login
- ? Menús diferenciados creados: MenuPrincipal (Admin), MenuProfesor, MenuAlumno  
- ? Modificación del Program.cs para redirigir según tipo de usuario
- ? Formularios específicos creados para cada rol

## ERRORES PENDIENTES DE CORREGIR

### 1. AuthEndpoints.cs - Métodos Sincrónicos
**Problema**: AuthEndpoints usa `GetByUsername()` en lugar de `GetByUsernameAsync()`

**Solución**: Cambiar:
```csharp
var user = usuarioRepo.GetByUsername(request.Username);
```
Por:
```csharp
var user = await usuarioRepo.GetByUsernameAsync(request.Username);
```

### 2. CursoDto - Propiedades Inconsistentes
**Problema**: Los formularios usan propiedades que no existen en CursoDto

**Estado actual de CursoDto**:
- ? NombreMateria (NO "Nombre")
- ? DescComision (NO "Comision")
- ? InscriptosActuales (NO "InscriptosCount")

**Acciones necesarias**: Agregar propiedades calculadas o actualizar el DTO:
```csharp
public string Nombre => NombreMateria ?? "Sin nombre";
public string Comision => DescComision ?? "Sin comisión";
public int InscriptosCount => InscriptosActuales ?? 0;
```

### 3. InscripcionApiClient - Métodos Faltantes
**Problema**: Los formularios usan métodos que no existen

**Métodos requeridos**:
- `GetByAlumnoIdAsync(int alumnoId)` - Para obtener cursos de un alumno
- `GetByCursoIdAsync(int cursoId)` - Para obtener alumnos de un curso
- `UpdateCondicionAsync(int id, CondicionAlumnoDto condicion, int? nota)` - Para actualizar notas
- `CreateAsync(AlumnoCursoDto dto)` - Para crear inscripciones

### 4. PlanDto - Propiedad IdEspecialidad
**Problema**: PlanDto no tiene `IdEspecialidad`

**Solución**: Verificar qué propiedad tiene (probablemente `EspecialidadId`) y usar esa

## FUNCIONALIDADES IMPLEMENTADAS

### Menú Administrador (MenuPrincipal)
- ? Gestión completa de usuarios
- ? Gestión de alumnos
- ? Gestión de profesores
- ? Gestión de especialidades, planes, comisiones, cursos
- ? Gestión de inscripciones
- ?? Asignar profesores a cursos (creado, pero requiere backend)
- ?? Reporte de planes (creado, requiere correcciones menores)
- ?? Botón "Reporte Futuro" (placeholder)

### Menú Profesor (MenuProfesor)
- ? Ver mis cursos asignados (FormMisCursosProfesor)
- ?? Cargar notas y condiciones (FormCargarNotasProfesor - requiere correcciones)
- ? Cerrar sesión

### Menú Alumno (MenuAlumno)
- ?? Mis cursos e inscripciones (FormMisCursosAlumno - requiere correcciones)
- ?? Inscribirse a curso (FormInscripcionAlumno - requiere correcciones)
- ? Cerrar sesión

## PASOS PARA COMPLETAR LA IMPLEMENTACIÓN

### Paso 1: Corregir AuthEndpoints.cs
```csharp
// Buscar en líneas 31, 118 y LoginTestHelper.cs línea 29
// Cambiar GetByUsername por GetByUsernameAsync
// Agregar await donde corresponda
```

### Paso 2: Actualizar CursoDto
```csharp
public class CursoDto
{
    public int IdCurso { get; set; }
    public int? IdMateria { get; set; }
    public string? NombreMateria { get; set; }
    public int IdComision { get; set; }
    public string? DescComision { get; set; }
    public int AnioCalendario { get; set; }
    public int Cupo { get; set; }
    public int? InscriptosActuales { get; set; }
    
    // Propiedades calculadas para compatibilidad
    public string Nombre => NombreMateria ?? "Sin nombre";
    public string Comision => DescComision ?? "Sin comisión";
    public int InscriptosCount => InscriptosActuales ?? 0;
}
```

### Paso 3: Implementar Métodos en InscripcionApiClient
Agregar en `InscripcionApiClient.cs`:

```csharp
public async Task<List<AlumnoCursoDto>> GetByAlumnoIdAsync(int alumnoId)
{
    using var httpClient = await CreateHttpClientAsync();
    var response = await httpClient.GetAsync($"/inscripciones/alumno/{alumnoId}");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    return JsonSerializer.Deserialize<List<AlumnoCursoDto>>(content, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    }) ?? new List<AlumnoCursoDto>();
}

public async Task<List<AlumnoCursoDto>> GetByCursoIdAsync(int cursoId)
{
    using var httpClient = await CreateHttpClientAsync();
    var response = await httpClient.GetAsync($"/inscripciones/curso/{cursoId}");
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    return JsonSerializer.Deserialize<List<AlumnoCursoDto>>(content, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    }) ?? new List<AlumnoCursoDto>();
}

public async Task UpdateCondicionAsync(int id, CondicionAlumnoDto condicion, int? nota)
{
    using var httpClient = await CreateHttpClientAsync();
    var dto = new { Condicion = condicion.ToString(), Nota = nota };
    var json = JsonSerializer.Serialize(dto);
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    var response = await httpClient.PutAsync($"/inscripciones/{id}/condicion", content);
    response.EnsureSuccessStatusCode();
}

public async Task CreateAsync(AlumnoCursoDto inscripcion)
{
    using var httpClient = await CreateHttpClientAsync();
    var json = JsonSerializer.Serialize(inscripcion);
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    var response = await httpClient.PostAsync("/inscripciones", content);
    response.EnsureSuccessStatusCode();
}
```

### Paso 4: Corregir PlanDto
Verificar cuál es el nombre correcto de la propiedad (probablemente `EspecialidadId` en lugar de `IdEspecialidad`)

### Paso 5: Corregir errores menores
- Agregar .ToList() donde se requiera convertir IEnumerable a List
- Corregir nullability warnings según sea necesario

## RESUMEN DE ARCHIVOS CREADOS

### WindowsForms (WIndowsForm)
1. **MenuAlumno.cs** - Menú principal para alumnos
2. **MenuProfesor.cs** - Menú principal para profesores
3. **FormMisCursosAlumno.cs** - Ver cursos en los que está inscripto el alumno
4. **FormInscripcionAlumno.cs** - Inscribirse a nuevos cursos
5. **FormMisCursosProfesor.cs** - Ver cursos asignados al profesor
6. **FormCargarNotasProfesor.cs** - Cargar notas y condiciones para alumnos
7. **FormAsignarProfesores.cs** - Asignar profesores a cursos (Admin)
8. **FormReportePlanes.cs** - Reporte de planes de estudio (Admin)

### DTOs Modificados
1. **LoginResponse.cs** - Agregado UserId, PersonaId, TipoUsuario

### Services Modificados
1. **WindowsFormsAuthService.cs** - Agregados métodos estáticos para obtener datos del usuario
2. **AuthService.cs** - Modificado LoginAsync para devolver tipo de usuario
3. **Program.cs** - Modificado para dirigir al menú correcto según tipo de usuario

## PRÓXIMOS PASOS RECOMENDADOS

1. Corregir errores de compilación en el orden indicado arriba
2. Probar cada funcionalidad por separado:
   - Login como administrador
   - Login como profesor (crear usuario profesor primero)
   - Login como alumno (crear usuario alumno primero)
3. Completar funcionalidad de asignar profesores a cursos (requiere tabla en BD)
4. Implementar el reporte futuro según necesidades

## NOTAS IMPORTANTES

- El sistema ya detecta automáticamente el tipo de usuario según PersonaId
- Los permisos se asignan automáticamente en el backend
- Cada menú muestra solo las opciones pertinentes al rol
- Los formularios están preparados para autocompletar datos del usuario logeado

---

**Estado**: Implementación al 70%
**Bloqueantes**: Errores de compilación por métodos faltantes en API Clients
**Siguiente acción**: Corregir InscripcionApiClient y CursoDto
