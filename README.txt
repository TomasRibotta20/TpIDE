Guia para usar la aplicacion:
1- utilizar docker para la base de datos con este container:
docker run --hostname=06d087f30eff --user=mssql --env=ACCEPT_EULA=Y --env=SA_PASSWORD=TuContraseñaFuerte123 --env=PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin --env=MSSQL_RPC_PORT=135 --env=CONFIG_EDGE_BUILD= --env=MSSQL_PID=developer --network=bridge -p 1433:1433 --restart=no --label='com.microsoft.product=Microsoft SQL Server' --label='com.microsoft.version=16.0.4215.2' --label='org.opencontainers.image.ref.name=ubuntu' --label='org.opencontainers.image.version=22.04' --label='vendor=Microsoft' --runtime=runc -d mcr.microsoft.com/mssql/server:2022-latest
2- Seleccionar como proyectos de inicio: AcademiaAPI y WindowsForm
3- Iniciar la aplicacion y dejar que cargue los datos basicos necesarios, asi como las tablas de la base de datos.
4- La primera vez loguearse con el usuario de nombre: admin y contrasenia:admin123
