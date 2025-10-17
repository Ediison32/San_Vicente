## Proyecto 
El Hospital San Vicente actualmente gestiona sus citas médicas en agendas físicas y hojas de
cálculo.

## Arquitectura  MVC

El proyecto está estructurado bajo el patrón **Modelo-Vista-Controlador**, lo que permite una separación clara de responsabilidades:

- **Modelo (Model):**  
  Contiene las clases y objetos que representan los datos del sistema (por ejemplo, `Vuelos`, `Pasajeros`, `Reservas`).  
  Aquí se definen los atributos, las relaciones y las reglas de negocio.

- **Vista (View):**  
  Corresponde a las plantillas HTML renderizadas con **Razor**, donde se muestra la información al usuario de manera dinámica e interactiva.

- **Controlador (Controller):**  
  Gestiona las peticiones HTTP (GET, POST, etc.), interactúa con los modelos y envía la información a las vistas.  
  Ejemplo: `VuelosController`, `PasajerosController`, `ReservasController`.


## ejecutar comando
- dotnet new mvc -n nameProyect   ==> crea un nuevo archivo completo de mvc

##  Tecnologías Utilizadas

- **ASP.NET Core MVC** – Framework principal para el desarrollo web.
- **Entity Framework Core (EF Core)** – ORM para la gestión de la base de datos.
- **Pomelo.EntityFrameworkCore.MySql** – Proveedor para conectar con bases de datos MySQL.
- **QuestPDF** – Librería para la generación de documentos PDF (tickets, reportes).
- **Bootstrap 5** – Framework CSS para el diseño visual y los modales.
- **C# 12 / .NET 8** – Lenguaje y entorno de ejecución del proyecto.

## conexion db
- Base de datos desplegada en **aiven**
- 1 INSTALAR  :: dotnet add package microsoft.EntityFrameworkCore
- 2 INSTALAR  :: dotnet add package microsoft.EntityFrameworkCore.design
-  3 INSTALAR  :: dotnet add package Pomelo.EntityFrameworkCore.Mysql
-  4  INSTALAR  :: dotnet tool install --global dotnet-ef
- Verificar que este importando Entity
- Configuraciones para la variable de entorno y traer la configuracion de la db
- para finalizar necesitamos ejeccutar los siguientes comando para crear la migracion y para crear la db en la base de datos }
-  dotnet ef migrations add InitialCreate
- dotnet ef database update

## gitignore:
- dotnet new gitignore

## Enviado de correo electronico  


## pdf
-  dotnet add package QuestPD

## codigo qr
-// instalar dotnet add package QRCoder


## Correr el proyecto

    -Verificar si tiene el archivo appsettings.json donde se encuentra toda la variables para la conexion con la db
    - Ejecutar : dotnet run.
    -Esperar que se despliegue. 
## Como cree el proyecto

    - Leer el taller 
    - crear las relaciones en un UML 
    - Crear los modelos con sus relaciones 

    - Crear el archivo Data par la configuracion de : AppDbContext
    - crear la configuracion en program de la db -- recordar que la posicionn de la configuracion puede afectar 

    - hacer su respectiva migracion con actualizacion 

    - Crear Controladores con las vistas


    - consultar que es Questpdf  ---> 