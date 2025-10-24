@echo off
echo =====================================
echo    EF Core - Crear Migracion Inicial
echo =====================================
echo.

cd /d "%~dp0"

echo Instalando/Actualizando herramientas EF Core...
dotnet tool update --global dotnet-ef
echo.

echo Creando migracion inicial...
dotnet ef migrations add InitialCreate --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
echo.

if %errorlevel% equ 0 (
    echo [OK] Migracion creada exitosamente!
    echo.
    
    set /p apply="Deseas aplicar la migracion ahora? (S/N): "
    if /i "%apply%"=="S" (
        echo.
        echo Aplicando migracion a la base de datos...
        dotnet ef database update --startup-project ..\AcademiaAPI\AcademiaAPI.csproj
        echo.
        
        if %errorlevel% equ 0 (
            echo [OK] Migracion aplicada exitosamente!
        ) else (
            echo [ERROR] No se pudo aplicar la migracion
        )
    ) else (
        echo.
        echo [INFO] La migracion se aplicara automaticamente al iniciar la API
    )
) else (
    echo [ERROR] No se pudo crear la migracion
)

echo.
echo =====================================
pause
