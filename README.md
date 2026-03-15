markdown
# 📌 TeamTasksSample

Repositorio de ejemplo para la creación y gestión de una base de datos de tareas de equipo en **SQL Server**, con un backend en **.NET 9** y un frontend en **Angular**.

---

## 📂 Contenido del repositorio

- `scripts/DBSetup_TeamTasks.sql`  
  Script principal para crear la base de datos, esquemas, tablas y datos de prueba.

- `TeamTasksApi/`  
  Proyecto backend en .NET para exponer la API REST.

- `UIs/team-tasks-dashboard/`  
  Proyecto frontend en Angular para consumir la API y mostrar la información.

---

## 🚀 Requisitos

### Backend
- .NET 9 (compatible con .NET 8+)
- SQL Server 2019 o superior
- SQL Server Management Studio (SSMS) o cliente compatible
- Permisos de administrador para crear bases de datos

### Frontend
- Node.js (versión LTS recomendada)
- Angular CLI instalado globalmente:
  bash
  npm install -g @angular/cli
⚙️ Instalación y ejecución
1. Configuración de la base de datos
Abrir SQL Server Management Studio.

Ejecutar el script scripts/DBSetup_TeamTasks.sql para crear la base de datos y datos de prueba.

2. Ejecutar el backend (API .NET)
Abrir una ventana de comandos en la carpeta del proyecto backend:

bash
cd TeamTasksApi
Restaurar paquetes y ejecutar la API:

bash
dotnet restore
dotnet run
La API quedará disponible en:

http://localhost:5200/api

Documentación Swagger: http://localhost:5200/swagger

3. Ejecutar el frontend (Angular)
Navegar a la carpeta del frontend:

bash
cd UIs/team-tasks-dashboard
Instalar dependencias:

bash
npm install
Iniciar el servidor de desarrollo:

bash
ng serve -o
Esto abrirá automáticamente el navegador en http://localhost:4200.

📦 Paquetes utilizados en el backend
Microsoft.EntityFrameworkCore (v9.0.14) – ORM para trabajar con SQL Server.

Microsoft.EntityFrameworkCore.SqlServer (v9.0.14) – Proveedor específico para SQL Server.

Microsoft.EntityFrameworkCore.Tools (v9.0.14) – Herramientas para migraciones y scaffolding.

Swashbuckle.AspNetCore – Genera documentación interactiva con Swagger/OpenAPI.

Ejemplo de migraciones:

bash
dotnet ef migrations add InitialCreate
dotnet ef database update
🔗 Endpoints principales del Dashboard
Proyectos
GET /api/dashboard/projects

Tareas de un proyecto
GET /api/dashboard/projects/{id}/tasks?page={page}&pageSize={pageSize}

Carga de trabajo por desarrollador
GET /api/dashboard/developer-load-summary

Estado de proyectos
GET /api/dashboard/project-status-summary

Tareas próximas a vencer
GET /api/dashboard/tasks-due-soon

Riesgo de retraso por desarrollador
GET /api/dashboard/developer-risk-summary

Desarrolladores activos
GET /api/dashboard/developers

Actualizar estado de una tarea
PUT /api/dashboard/tasks/{id}/status

Crear nueva tarea (procedimiento almacenado)
POST /api/dashboard/tasks

🖥️ Navegabilidad del Frontend
El frontend en Angular ofrece las siguientes vistas:

Dashboard principal: muestra carga de trabajo, riesgos y estado de proyectos.

Vista de proyectos: lista de proyectos disponibles.

Vista de tareas de un proyecto: tareas filtradas por proyecto con paginación.

Formulario de nueva tarea: permite crear una tarea asignando desarrollador, estado, prioridad y fecha de vencimiento.

Actualización de estado de tarea: modificar estado, prioridad o complejidad.

📌 Notas
Los endpoints de vistas (summary) dependen de consultas predefinidas en la base de datos.

La creación de tareas se realiza mediante un procedimiento almacenado para garantizar consistencia.

La paginación en tareas de proyectos se controla con parámetros page y pageSize.

El frontend consume directamente los endpoints del backend y muestra mensajes de validación en los formularios.