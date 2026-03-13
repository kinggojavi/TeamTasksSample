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
