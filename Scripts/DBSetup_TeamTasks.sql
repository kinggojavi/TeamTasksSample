
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

-- Insertar datos de prueba
-- Insertar datos de prueba
INSERT INTO TeamTasks.Developers (FirstName, LastName, Email)
VALUES
('Jorge','Pérez','jorge.perez@anymail.com'),
('Camila','Rojas','camila.rojas@anymail.com'),
('Andrés','Moreno','andres.moreno@anymail.com'),
('Valentina','Castro','valentina.castro@anymail.com'),
('Felipe','Ramírez','felipe.ramirez@anymail.com');

INSERT INTO TeamTasks.Projects (Name, ClientName, StartDate, EndDate, Status)
VALUES
('ERP Financiero','Banco Central','2026-01-10','2026-07-15','InProgress'),
('E-commerce','RetailMax','2026-02-01','2026-09-01','Planned'),
('Sistema Académico','Universidad Nacional','2026-03-05','2026-10-30','Planned');

-- Insertar 20 tareas distribuidas
INSERT INTO TeamTasks.Tasks (ProjectId, Title, Description, AssigneeId, Status, Priority, EstimatedComplexity, DueDate)
VALUES
(1,'Modelo Contable','Diseñar tablas contables',1,'InProgress','High',5,'2026-03-25'),
(1,'Integración Pagos','Conectar con pasarela',2,'ToDo','Medium',3,'2026-04-01'),
(1,'Reportes Financieros','Generar reportes mensuales',3,'ToDo','High',4,'2026-04-10'),
(1,'Seguridad','Configurar roles y permisos',4,'ToDo','High',5,'2026-04-15'),
(1,'Manual Usuario','Redactar guía funcional',5,'ToDo','Low',2,'2026-04-20'),

(2,'Catálogo Productos','Diseñar base de datos',1,'ToDo','Medium',3,'2026-05-01'),
(2,'Carrito Compras','Implementar lógica',2,'ToDo','High',4,'2026-05-05'),
(2,'Checkout','Integrar pagos',3,'ToDo','High',5,'2026-05-10'),
(2,'Dashboard Admin','Diseñar panel control',4,'ToDo','Medium',3,'2026-05-15'),
(2,'Pruebas Funcionales','QA inicial',5,'ToDo','Medium',2,'2026-05-20'),

(3,'Gestión Estudiantes','CRUD estudiantes',1,'InProgress','High',4,'2026-06-01'),
(3,'Gestión Cursos','CRUD cursos',2,'ToDo','Medium',3,'2026-06-05'),
(3,'Notas','Registrar calificaciones',3,'ToDo','Medium',3,'2026-06-10'),
(3,'Portal Docentes','Diseñar interfaz',4,'ToDo','Low',2,'2026-06-15'),
(3,'Reportes Académicos','Generar informes',5,'ToDo','Medium',3,'2026-06-20'),

(1,'Auditoría','Configurar logs',1,'ToDo','Medium',3,'2026-07-01'),
(2,'SEO','Optimizar catálogo',2,'ToDo','Low',2,'2026-07-05'),
(3,'Biblioteca Virtual','Integrar sistema externo',3,'ToDo','High',5,'2026-07-10'),
(2,'Notificaciones','Configurar email y SMS',4,'ToDo','Medium',3,'2026-07-15'),
(3,'Backup Académico','Configurar respaldos',5,'ToDo','Medium',2,'2026-07-20');
