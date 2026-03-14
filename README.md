\# TeamTasksSample



Repositorio de ejemplo para la creación y gestión de una base de datos de tareas de equipo en \*\*SQL Server\*\*.



\## 📌 Contenido

\- `scripts/DBSetup\_TeamTasks.sql`: Script principal para crear la base de datos, esquemas, tablas y datos de prueba.

\- Futuras carpetas podrán incluir documentación, ejemplos de consultas y scripts adicionales.



\## 🚀 Requisitos

\- SQL Server 2019 o superior (funciona también en versiones anteriores con ajustes mínimos).

\- SQL Server Management Studio (SSMS) o cualquier cliente compatible.

\- Permisos de administrador para crear bases de datos.



\## ⚙️ Instalación

1\. Clona este repositorio:

&nbsp;  ```bash

&nbsp;  git clone https://github.com/kinggojavi/TeamTasksSample.git

&nbsp;  cd TeamTasksSample/scripts


📌 Reglas asumidas (VIEW TeamTasks.vw_DeveloperRiskSummary)

1. AvgDelayDays:

    Se calcula solo sobre tareas completadas.

    Si se completó antes o en fecha, el retraso es 0.

    Si no hay tareas completadas, se asume 0.


2. PredictedCompletionDate:

    Se estima como LatestDueDate + AvgDelayDays.

    Si AvgDelayDays = 0, se asume que se cumplirá en la fecha original.


3. HighRiskFlag:

    Se marca 1 si la fecha estimada de completitud supera la última fecha de vencimiento (PredictedCompletionDate > LatestDueDate).

    También se marca 1 si el promedio de retraso (AvgDelayDays) es mayor a 5 días (umbral razonable).

    En otros casos, 0.


## 📦 Paquetes utilizados

La API se construyó en **.NET 9 (compatible con .NET 8+)** y utiliza los siguientes paquetes:

- **Microsoft.EntityFrameworkCore (v9.0.14)**  
  Núcleo de Entity Framework Core. Proporciona el ORM (Object-Relational Mapping) que permite trabajar con la base de datos mediante clases y LINQ en lugar de SQL directo.

- **Microsoft.EntityFrameworkCore.SqlServer (v9.0.14)**  
  Proveedor específico para SQL Server. Permite que EF Core se conecte y traduzca las consultas LINQ a instrucciones SQL optimizadas para SQL Server.

- **Microsoft.EntityFrameworkCore.Tools (v9.0.14)**  
  Herramientas de línea de comandos para trabajar con migraciones y scaffolding.  
  Ejemplos de uso:
  - Crear una migración inicial:  
    ```bash
    dotnet ef migrations add InitialCreate
    ```
  - Aplicar migraciones a la base de datos:  
    ```bash
    dotnet ef database update
    ```

- **Swashbuckle.AspNetCore (última versión estable)**  
  Genera documentación interactiva de la API con Swagger/OpenAPI.  
  Permite probar los endpoints directamente desde el navegador en la ruta `/swagger`.

---

### 🔧 Instalación

```bash
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.14
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.14
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.14
dotnet add package Swashbuckle.AspNetCore


# TeamTasks API - Endpoints del Dashboard

Este controlador expone diferentes endpoints para gestionar proyectos, tareas y vistas relacionadas con el estado del equipo.

## Endpoints

### 1. Proyectos
- **GET** `/api/dashboard/projects`  
  Obtiene todos los proyectos.

### 2. Tareas de un proyecto
- **GET** `/api/dashboard/projects/{id}/tasks?page={page}&pageSize={pageSize}`  
  Obtiene las tareas de un proyecto específico con paginación.

### 3. Carga de trabajo por desarrollador
- **GET** `/api/dashboard/developer-load-summary`  
  Vista con el resumen de carga de trabajo por desarrollador.

### 4. Estado de proyectos
- **GET** `/api/dashboard/project-status-summary`  
  Vista con el resumen del estado de los proyectos.

### 5. Tareas próximas a vencer
- **GET** `/api/dashboard/tasks-due-soon`  
  Vista con las tareas que están próximas a vencer.

### 6. Riesgo de retraso por desarrollador
- **GET** `/api/dashboard/developer-risk-summary`  
  Vista con el riesgo de retraso por desarrollador.

### 7. Desarrolladores activos
- **GET** `/api/dashboard/developers`  
  Obtiene los desarrolladores activos con su información básica (ID, nombre completo y correo).

### 8. Actualizar estado de una tarea
- **PUT** `/api/dashboard/tasks/{id}/status`  
  Actualiza el estado, prioridad o complejidad estimada de una tarea.  
  **Body:** `TaskUpdateDto`

### 9. Crear una nueva tarea (procedimiento almacenado)
- **POST** `/api/dashboard/tasks`  
  Crea una nueva tarea usando el procedimiento almacenado `TeamTasks.InsertTask`.  
  **Body:** `TaskItemInsertDto`

---

## Notas
- Los endpoints de vistas (`summary`) dependen de consultas predefinidas en la base de datos.  
- La creación de tareas se realiza mediante un procedimiento almacenado para garantizar consistencia en la inserción.  
- La paginación en tareas de proyectos se controla con parámetros `page` y `pageSize`.


