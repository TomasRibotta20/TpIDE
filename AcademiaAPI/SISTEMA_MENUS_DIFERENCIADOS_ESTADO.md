# SISTEMA DE MENUS DIFERENCIADOS - IMPLEMENTACION PARCIAL

## ESTADO ACTUAL

El sistema tiene implementado:
- ? Detecci�n autom�tica del tipo de usuario en el login
- ? Men�s diferenciados creados: MenuPrincipal (Admin), MenuProfesor, MenuAlumno  
- ? Modificaci�n del Program.cs para redirigir seg�n tipo de usuario
- ? Formularios espec�ficos creados para cada rol

## ERRORES PENDIENTES DE CORREGIR

### 1. AuthEndpoints.cs - M�todos Sincr�nicos
**Problema**: AuthEndpoints usa `GetByUsername()` en lugar de `GetByUsernameAsync()`

**Soluci�n**: Cambiar:
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
public string Comision => DescComision ?? "Sin comisi�n";
public int InscriptosCount => InscriptosActuales ?? 0;
```

### 3. InscripcionApiClient - M�todos Faltantes
**Problema**: Los formularios usan m�todos que no existen

**M�todos requeridos**:
- `GetByAlumnoIdAsync(int alumnoId)` - Para obtener cursos de un alumno
- `GetByCursoIdAsync(int cursoId)` - Para obtener alumnos de un curso
- `UpdateCondicionAsync(int id, CondicionAlumnoDto condicion, int? nota)` - Para actualizar notas
- `CreateAsync(AlumnoCursoDto dto)` - Para crear inscripciones

### 4. PlanDto - Propiedad IdEspecialidad
**Problema**: PlanDto no tiene `IdEspecialidad`

**Soluci�n**: Verificar qu� propiedad tiene (probablemente `EspecialidadId`) y usar esa

## FUNCIONALIDADES IMPLEMENTADAS

### Men� Administrador (MenuPrincipal)
- ? Gesti�n completa de usuarios
- ? Gesti�n de alumnos
- ? Gesti�n de profesores
- ? Gesti�n de especialidades, planes, comisiones, cursos
- ? Gesti�n de inscripciones
- ?? Asignar profesores a cursos (creado, pero requiere backend)
- ?? Reporte de planes (creado, requiere correcciones menores)
- ?? Bot�n "Reporte Futuro" (placeholder)

### Men� Profesor (MenuProfesor)
- ? Ver mis cursos asignados (FormMisCursosProfesor)
- ?? Cargar notas y condiciones (FormCargarNotasProfesor - requiere correcciones)
- ? Cerrar sesi�n

### Men� Alumno (MenuAlumno)
- ?? Mis cursos e inscripciones (FormMisCursosAlumno - requiere correcciones)
- ?? Inscribirse a curso (FormInscripcionAlumno - requiere correcciones)
- ? Cerrar sesi�n

## PASOS PARA COMPLETAR LA IMPLEMENTACI�N

### Paso 1: Corregir AuthEndpoints.cs
```csharp
// Buscar en l�neas 31, 118 y LoginTestHelper.cs l�nea 29
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
    public string Comision => DescComision ?? "Sin comisi�n";
    public int InscriptosCount => InscriptosActuales ?? 0;
}
```

### Paso 3: Implementar M�todos en InscripcionApiClient
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
Verificar cu�l es el nombre correcto de la propiedad (probablemente `EspecialidadId` en lugar de `IdEspecialidad`)

### Paso 5: Corregir errores menores
- Agregar .ToList() donde se requiera convertir IEnumerable a List
- Corregir nullability warnings seg�n sea necesario

## RESUMEN DE ARCHIVOS CREADOS

### WindowsForms (WIndowsForm)
1. **MenuAlumno.cs** - Men� principal para alumnos
2. **MenuProfesor.cs** - Men� principal para profesores
3. **FormMisCursosAlumno.cs** - Ver cursos en los que est� inscripto el alumno
4. **FormInscripcionAlumno.cs** - Inscribirse a nuevos cursos
5. **FormMisCursosProfesor.cs** - Ver cursos asignados al profesor
6. **FormCargarNotasProfesor.cs** - Cargar notas y condiciones para alumnos
7. **FormAsignarProfesores.cs** - Asignar profesores a cursos (Admin)
8. **FormReportePlanes.cs** - Reporte de planes de estudio (Admin)

### DTOs Modificados
1. **LoginResponse.cs** - Agregado UserId, PersonaId, TipoUsuario

### Services Modificados
1. **WindowsFormsAuthService.cs** - Agregados m�todos est�ticos para obtener datos del usuario
2. **AuthService.cs** - Modificado LoginAsync para devolver tipo de usuario
3. **Program.cs** - Modificado para dirigir al men� correcto seg�n tipo de usuario

## PR�XIMOS PASOS RECOMENDADOS

1. Corregir errores de compilaci�n en el orden indicado arriba
2. Probar cada funcionalidad por separado:
   - Login como administrador
   - Login como profesor (crear usuario profesor primero)
   - Login como alumno (crear usuario alumno primero)
3. Completar funcionalidad de asignar profesores a cursos (requiere tabla en BD)
4. Implementar el reporte futuro seg�n necesidades

## NOTAS IMPORTANTES

- El sistema ya detecta autom�ticamente el tipo de usuario seg�n PersonaId
- Los permisos se asignan autom�ticamente en el backend
- Cada men� muestra solo las opciones pertinentes al rol
- Los formularios est�n preparados para autocompletar datos del usuario logeado

---

**Estado**: Implementaci�n al 70%
**Bloqueantes**: Errores de compilaci�n por m�todos faltantes en API Clients
**Siguiente acci�n**: Corregir InscripcionApiClient y CursoDto
