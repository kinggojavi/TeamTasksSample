
--RUN FIRST THIS CREATE DATABASE SCRIPT
CREATE DATABASE TeamTasksSample;
GO
-------------------------------------------

USE TeamTasksSample;
GO

-- Crear esquema
CREATE SCHEMA TeamTasks AUTHORIZATION dbo;
GO

-- Tabla Developers
CREATE TABLE TeamTasks.Developers (
    DeveloperId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

-- Tabla Projects
CREATE TABLE TeamTasks.Projects (
    ProjectId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    ClientName NVARCHAR(100) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NULL,
    Status NVARCHAR(20) CHECK (Status IN ('Planned','InProgress','Completed')) NOT NULL
);

-- Tabla Tasks
CREATE TABLE TeamTasks.Tasks (
    TaskId INT IDENTITY(1,1) PRIMARY KEY,
    ProjectId INT NOT NULL FOREIGN KEY REFERENCES TeamTasks.Projects(ProjectId),
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    AssigneeId INT NOT NULL FOREIGN KEY REFERENCES TeamTasks.Developers(DeveloperId),
    Status NVARCHAR(20) CHECK (Status IN ('ToDo','InProgress','Blocked','Completed')) NOT NULL,
    Priority NVARCHAR(10) CHECK (Priority IN ('Low','Medium','High')) NOT NULL,
    EstimatedComplexity INT CHECK (EstimatedComplexity BETWEEN 1 AND 5),
    DueDate DATE NULL,
    CompletionDate DATE NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);



-- Insertar desarrolladores
INSERT INTO TeamTasks.Developers (FirstName, LastName, Email)
VALUES
('Jorge','Pérez','jorge.perez@anymail.com'),
('Camila','Rojas','camila.rojas@anymail.com'),
('Andrés','Moreno','andres.moreno@anymail.com'),
('Valentina','Castro','valentina.castro@anymail.com'),
('Felipe','Ramírez','felipe.ramirez@anymail.com'),
('Camila','Arciniegas','camila.arc@anymail.com');

-- Insertar proyectos
INSERT INTO TeamTasks.Projects (Name, ClientName, StartDate, EndDate, Status)
VALUES
('ERP Financiero','Banco Central','2026-01-10','2026-07-15','InProgress'),
('E-commerce','RetailMax','2026-02-01','2026-09-01','Planned'),
('Sistema Académico','Universidad Nacional','2026-03-05','2026-10-30','Planned'),
('Gestión Hospitalaria','Clínica Santa María','2026-04-20','2026-12-15','Completed');

-- Insertar tareas distribuidas
INSERT INTO TeamTasks.Tasks (ProjectId, Title, Description, AssigneeId, Status, Priority, EstimatedComplexity, DueDate, CompletionDate)
VALUES
-- Proyecto 1: ERP Financiero
(1,'Modelo Contable','Diseñar tablas contables',1,'InProgress','High',5,'2026-03-25',NULL),
(1,'Integración Pagos','Conectar con pasarela',2,'ToDo','Medium',3,'2026-04-01',NULL),
(1,'Reportes Financieros','Generar reportes mensuales',3,'ToDo','High',4,'2026-04-10',NULL),
(1,'Seguridad','Configurar roles y permisos',4,'ToDo','High',5,'2026-04-15',NULL),
(1,'Manual Usuario','Redactar guía funcional',5,'ToDo','Low',2,'2026-04-20',NULL),

-- Proyecto 2: E-commerce
(2,'Catálogo Productos','Diseñar base de datos',1,'ToDo','Medium',3,'2026-05-01',NULL),
(2,'Carrito Compras','Implementar lógica',2,'ToDo','High',4,'2026-05-05',NULL),
(2,'Checkout','Integrar pagos',3,'ToDo','High',5,'2026-05-10',NULL),
(2,'Dashboard Admin','Diseñar panel control',4,'ToDo','Medium',3,'2026-05-15',NULL),
(2,'Pruebas Funcionales','QA inicial',5,'ToDo','Medium',2,'2026-05-20',NULL),

-- Proyecto 3: Sistema Académico
(3,'Gestión Estudiantes','CRUD estudiantes',1,'InProgress','High',4,'2026-06-01',NULL),
(3,'Gestión Cursos','CRUD cursos',2,'ToDo','Medium',3,'2026-06-05',NULL),
(3,'Notas','Registrar calificaciones',3,'ToDo','Medium',3,'2026-06-10',NULL),
(3,'Portal Docentes','Diseñar interfaz',4,'ToDo','Low',2,'2026-06-15',NULL),
(3,'Reportes Académicos','Generar informes',5,'ToDo','Medium',3,'2026-06-20',NULL),

-- Extras
(1,'Auditoría','Configurar logs',1,'ToDo','Medium',3,'2026-07-01',NULL),
(2,'SEO','Optimizar catálogo',2,'ToDo','Low',2,'2026-07-05',NULL),
(3,'Biblioteca Virtual','Integrar sistema externo',3,'ToDo','High',5,'2026-07-10',NULL),
(2,'Notificaciones','Configurar email y SMS',4,'ToDo','Medium',3,'2026-07-15',NULL),
(3,'Backup Académico','Configurar respaldos',5,'ToDo','Medium',2,'2026-07-20',NULL),

-- Proyecto 4: Gestión Hospitalaria (Completed)
(4,'Historia Clínica','Implementar módulo de historias clínicas',1,'Completed','High',5,'2026-05-15','2026-05-14'),
(4,'Agenda Médica','Configurar agenda de citas',2,'Completed','Medium',3,'2026-06-01','2026-05-30'),
(4,'Farmacia','Integrar inventario de medicamentos',3,'Completed','High',4,'2026-06-20','2026-06-18'),
(4,'Facturación','Automatizar procesos de cobro',4,'Completed','Medium',3,'2026-07-05','2026-07-03'),
(4,'Reportes Hospitalarios','Generar informes de gestión',5,'Completed','Low',2,'2026-07-15','2026-07-14'),

-- Caso 1: HighRiskFlag = 1 por AvgDelayDays > 3 (Jorge Pérez, DeveloperId=1)
(4,'Cirugía General','Registrar procedimientos quirúrgicos',1,'Completed','High',5,'2026-05-01','2026-05-10'), -- 9 días retraso
(4,'Urgencias','Configurar módulo de urgencias',1,'Completed','Medium',3,'2026-05-15','2026-05-25'), -- 10 días retraso
(4,'Laboratorio','Integrar resultados de laboratorio',1,'Completed','Medium',3,'2026-06-01','2026-06-08'), -- 7 días retraso

-- Caso 2: HighRiskFlag = 1 por PredictedCompletionDate > LatestDueDate (Camila Rojas, DeveloperId=2)
(4,'Consultas Externas','Configurar módulo de consultas',2,'Completed','Medium',3,'2026-06-10','2026-06-12'), -- 2 días retraso
(4,'Hospitalización','Configurar módulo de hospitalización',2,'ToDo','High',4,'2026-06-20',NULL),
(4,'Imagenología','Integrar sistema de radiología',2,'ToDo','Medium',3,'2026-06-25',NULL);
