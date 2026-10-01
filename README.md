Minimercado - ASP.NET Core

Sistema web desarrollado con ASP.NET Core, Entity Framework Core y SQL Server LocalDB para la gestión integral de un minimercado (control de productos y categorización).

📋 Prerrequisitos

Antes de ejecutar el proyecto en una PC nueva, asegúrate de tener instalado:

.NET SDK (.NET 10.0 o superior compatible).

SQL Server LocalDB (generalmente incluido con Visual Studio o SQL Server Express).

🚀 Guía rápida para ejecutar el proyecto

Sigue estos pasos en tu terminal (PowerShell o CMD) para poner en marcha la aplicación desde cero:

1. Clonar el repositorio

Abre tu terminal y clona el proyecto en tu máquina local:

git clone https://github.com/AlexCampesato/Minimercado.git
cd Minimercado


2. Restaurar las dependencias

Descarga todos los paquetes NuGet requeridos por la solución:

dotnet restore


3. Configurar y actualizar la Base de Datos

Para generar la base de datos LocalDB con sus respectivas tablas y el seeder automático de categorías, ejecuta:

dotnet ef database update --project Eventos.Data --startup-project Eventos.Web


(Nota: Si la terminal te indica que el comando dotnet-ef no está disponible, instálalo globalmente ejecutando: dotnet tool install --global dotnet-ef)

4. Ejecutar la aplicación web

Inicia el servidor local apuntando directamente al proyecto web:

dotnet run --project Eventos.Web


5. Acceder al sistema

Una vez que el servidor esté activo (verás los puertos HTTPS en la terminal, por ejemplo https://localhost:7039), abre tu navegador web y entra a esa dirección para comenzar a utilizar el minimercado.
