# Script para limpiar y reconstruir el proyecto Windows Forms

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "LIMPIEZA Y RECONSTRUCCIÓN" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# 1. Navegar al directorio del proyecto
Set-Location "$PSScriptRoot\..\WIndowsForm"
Write-Host "Directorio actual: $(Get-Location)" -ForegroundColor Yellow

# 2. Cerrar procesos que puedan estar usando los archivos
Write-Host ""
Write-Host "Buscando procesos de WIndowsForm..." -ForegroundColor Yellow
$processes = Get-Process | Where-Object { $_.ProcessName -like "*WIndowsForm*" }
if ($processes) {
    Write-Host "Cerrando procesos encontrados..." -ForegroundColor Red
    $processes | ForEach-Object { Stop-Process $_ -Force }
    Start-Sleep -Seconds 2
} else {
    Write-Host "No se encontraron procesos activos" -ForegroundColor Green
}

# 3. Eliminar carpetas bin y obj
Write-Host ""
Write-Host "Eliminando carpetas bin y obj..." -ForegroundColor Yellow

if (Test-Path "bin") {
    Remove-Item "bin" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "  - Carpeta bin eliminada" -ForegroundColor Green
}

if (Test-Path "obj") {
    Remove-Item "obj" -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "  - Carpeta obj eliminada" -ForegroundColor Green
}

# 4. Verificar que appsettings.json tiene la configuración correcta
Write-Host ""
Write-Host "Verificando appsettings.json..." -ForegroundColor Yellow
$appsettings = Get-Content "appsettings.json" -Raw | ConvertFrom-Json
$baseUrl = $appsettings.ApiSettings.BaseUrl

if ($baseUrl -eq "http://localhost:5000") {
    Write-Host "  ? BaseUrl correcto: $baseUrl" -ForegroundColor Green
} else {
    Write-Host "  ? BaseUrl incorrecto: $baseUrl" -ForegroundColor Red
    Write-Host "  Actualizando a http://localhost:5000..." -ForegroundColor Yellow
    
    $appsettings.ApiSettings.BaseUrl = "http://localhost:5000"
    $appsettings | ConvertTo-Json -Depth 10 | Set-Content "appsettings.json"
    
    Write-Host "  ? BaseUrl actualizado" -ForegroundColor Green
}

# 5. Limpiar con dotnet
Write-Host ""
Write-Host "Ejecutando dotnet clean..." -ForegroundColor Yellow
dotnet clean --verbosity quiet
Write-Host "  ? Limpieza completada" -ForegroundColor Green

# 6. Restaurar paquetes
Write-Host ""
Write-Host "Restaurando paquetes NuGet..." -ForegroundColor Yellow
dotnet restore --verbosity quiet
Write-Host "  ? Paquetes restaurados" -ForegroundColor Green

# 7. Compilar
Write-Host ""
Write-Host "Compilando proyecto..." -ForegroundColor Yellow
$buildOutput = dotnet build --no-restore 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host "  ? Compilación exitosa" -ForegroundColor Green
} else {
    Write-Host "  ? Error en compilación" -ForegroundColor Red
    Write-Host $buildOutput -ForegroundColor Red
    exit 1
}

# 8. Verificar que appsettings.json se copió a bin/Debug
Write-Host ""
Write-Host "Verificando copia de appsettings.json..." -ForegroundColor Yellow
$outputPath = "bin\Debug\net8.0-windows\appsettings.json"

if (Test-Path $outputPath) {
    $outputSettings = Get-Content $outputPath -Raw | ConvertFrom-Json
    $outputBaseUrl = $outputSettings.ApiSettings.BaseUrl
    
    if ($outputBaseUrl -eq "http://localhost:5000") {
        Write-Host "  ? appsettings.json copiado correctamente" -ForegroundColor Green
        Write-Host "  ? BaseUrl en output: $outputBaseUrl" -ForegroundColor Green
    } else {
        Write-Host "  ? BaseUrl incorrecto en output: $outputBaseUrl" -ForegroundColor Red
    }
} else {
    Write-Host "  ? appsettings.json NO se copió a bin/Debug" -ForegroundColor Red
}

# 9. Resumen
Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "PROCESO COMPLETADO" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Próximos pasos:" -ForegroundColor Yellow
Write-Host "1. Ejecuta la API (proyecto AcademiaAPI)" -ForegroundColor White
Write-Host "2. Espera a que inicie en http://localhost:5000" -ForegroundColor White
Write-Host "3. Ejecuta este proyecto Windows Forms" -ForegroundColor White
Write-Host ""
