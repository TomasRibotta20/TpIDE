# Crear una Nueva Migraci�n

# IMPORTANTE: Ejecuta estos comandos desde la ra�z del proyecto AcademiaAPI

# Navegar a la carpeta Data
cd ..\Data

# Crear una nueva migraci�n con un nombre descriptivo
# Reemplaza "NombreDeTuMigracion" con algo descriptivo como:
# - AgregaCampoTelefonoAlternoAPersona
# - AgregaTablaInscripciones  
# - ModificaRelacionPlanComision

dotnet ef migrations add NombreDeTuMigracion --startup-project ..\AcademiaAPI\AcademiaAPI.csproj

# Volver a la carpeta de la API
cd ..\AcademiaAPI

# Ahora simplemente ejecuta el proyecto (F5) y la migraci�n se aplicar� autom�ticamente

# ===========================
# COMANDOS �TILES ADICIONALES
# ===========================

# Ver lista de migraciones
# cd ..\Data
# dotnet ef migrations list --startup-project ..\AcademiaAPI\AcademiaAPI.csproj

# Ver el SQL que generar� una migraci�n (sin aplicarla)
# cd ..\Data
# dotnet ef migrations script --startup-project ..\AcademiaAPI\AcademiaAPI.csproj

# Eliminar la �ltima migraci�n (CUIDADO: puede perder datos)
# cd ..\Data
# dotnet ef migrations remove --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
