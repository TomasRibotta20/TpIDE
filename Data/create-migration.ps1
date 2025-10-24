# Script para crear y aplicar migraciones de Entity Framework Core
# Uso: .\create-migration.ps1 "NombreDeLaMigracion"

param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName
)

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "   EF Core Migration Tool" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# Cambiar al directorio del proyecto Data
$dataProjectPath = "D:\Documents\Desktop\Tarea\UTN\3er_anio\IDE\TPI(Entrega_2)\Data"
$startupProjectPath = "..\AcademiaAPI\AcademiaAPI.csproj"

Write-Host "?? Cambiando al directorio: $dataProjectPath" -ForegroundColor Yellow
Set-Location $dataProjectPath

Write-Host ""
Write-Host "?? Creando migración: $MigrationName" -ForegroundColor Green
dotnet ef migrations add $MigrationName --startup-project $startupProjectPath

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "? Migración creada exitosamente!" -ForegroundColor Green
    Write-Host ""
    
    $apply = Read-Host "¿Deseas aplicar la migración ahora? (S/N)"
    
    if ($apply -eq "S" -or $apply -eq "s") {
        Write-Host ""
        Write-Host "?? Aplicando migración a la base de datos..." -ForegroundColor Yellow
        dotnet ef database update --startup-project $startupProjectPath
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host ""
            Write-Host "? Migración aplicada exitosamente!" -ForegroundColor Green
        } else {
            Write-Host ""
            Write-Host "? Error al aplicar la migración" -ForegroundColor Red
        }
    } else {
        Write-Host ""
        Write-Host "??  La migración se aplicará automáticamente al iniciar la API" -ForegroundColor Cyan
    }
} else {
    Write-Host ""
    Write-Host "? Error al crear la migración" -ForegroundColor Red
}

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
