-- ============================================
-- VERIFICACIÓN Y DATOS DE PRUEBA
-- Tabla: docentes_cursos
-- ============================================

-- 1. Verificar estructura de la tabla
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'docentes_cursos'
ORDER BY ORDINAL_POSITION;

-- 2. Verificar índices
SELECT 
    i.name AS IndexName,
    i.is_unique AS IsUnique,
    c.name AS ColumnName
FROM sys.indexes i
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE i.object_id = OBJECT_ID('docentes_cursos')
ORDER BY i.name, ic.key_ordinal;

-- 3. Verificar claves foráneas
SELECT 
    fk.name AS ForeignKeyName,
    OBJECT_NAME(fk.parent_object_id) AS TableName,
    COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS ColumnName,
    OBJECT_NAME(fk.referenced_object_id) AS ReferencedTable,
    COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS ReferencedColumn,
    fk.delete_referential_action_desc AS DeleteAction
FROM sys.foreign_keys AS fk
INNER JOIN sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
WHERE fk.parent_object_id = OBJECT_ID('docentes_cursos');

-- ============================================
-- DATOS DE PRUEBA (OPCIONAL)
-- ============================================

-- Primero verificar que existan cursos y profesores
PRINT '=== CURSOS DISPONIBLES ===';
SELECT IdCurso, IdMateria, IdComision, AnioCalendario, Cupo
FROM Cursos;

PRINT '=== PROFESORES DISPONIBLES ===';
SELECT Id, Nombre, Apellido, Legajo, TipoPersona
FROM Personas
WHERE TipoPersona = 'Profesor';

-- Ejemplo de INSERT (ajustar IDs según datos reales)
-- Descomenta y ajusta los IDs según corresponda:

/*
-- Asignar Jefe de Cátedra al curso 1
INSERT INTO docentes_cursos (id_curso, id_docente, cargo)
VALUES (1, 1, 'JefeDeCatedra');

-- Asignar Titular al curso 1
INSERT INTO docentes_cursos (id_curso, id_docente, cargo)
VALUES (1, 2, 'Titular');

-- Asignar Auxiliar al curso 1
INSERT INTO docentes_cursos (id_curso, id_docente, cargo)
VALUES (1, 3, 'Auxiliar');

-- Verificar asignaciones creadas
SELECT 
    dc.id_dictado,
    dc.id_curso,
    p.Apellido + ', ' + p.Nombre AS Docente,
    dc.cargo
FROM docentes_cursos dc
INNER JOIN Personas p ON dc.id_docente = p.Id
ORDER BY dc.id_curso, dc.cargo;
*/

-- Consulta para ver todas las asignaciones con información completa
SELECT 
    dc.id_dictado AS ID,
    c.IdCurso AS CursoID,
    m.Descripcion AS Materia,
    com.DescComision AS Comision,
    c.AnioCalendario AS Año,
    p.Apellido + ', ' + p.Nombre AS Docente,
    p.Legajo,
    dc.cargo AS Cargo
FROM docentes_cursos dc
INNER JOIN Cursos c ON dc.id_curso = c.IdCurso
LEFT JOIN Materias m ON c.IdMateria = m.Id
INNER JOIN Comisiones com ON c.IdComision = com.IdComision
INNER JOIN Personas p ON dc.id_docente = p.Id
ORDER BY c.IdCurso, 
    CASE dc.cargo 
        WHEN 'JefeDeCatedra' THEN 1 
        WHEN 'Titular' THEN 2 
        WHEN 'Auxiliar' THEN 3 
    END;
