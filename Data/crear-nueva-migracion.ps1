# Crear una Nueva Migración

# IMPORTANTE: Ejecuta estos comandos desde la raíz del proyecto AcademiaAPI

# Navegar a la carpeta Data
cd ..\Data

# Crear una nueva migración con un nombre descriptivo
# Reemplaza "NombreDeTuMigracion" con algo descriptivo como:
# - AgregaCampoTelefonoAlternoAPersona
# - AgregaTablaInscripciones  
# - ModificaRelacionPlanComision

dotnet ef migrations add NombreDeTuMigracion --startup-project ..\AcademiaAPI\AcademiaAPI.csproj

# Volver a la carpeta de la API
cd ..\AcademiaAPI

# Ahora simplemente ejecuta el proyecto (F5) y la migración se aplicará automáticamente

# ===========================
# COMANDOS ÚTILES ADICIONALES
# ===========================

# Ver lista de migraciones
# cd ..\Data
# dotnet ef migrations list --startup-project ..\AcademiaAPI\AcademiaAPI.csproj

# Ver el SQL que generará una migración (sin aplicarla)
# cd ..\Data
# dotnet ef migrations script --startup-project ..\AcademiaAPI\AcademiaAPI.csproj

# Eliminar la última migración (CUIDADO: puede perder datos)
# cd ..\Data
# dotnet ef migrations remove --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
