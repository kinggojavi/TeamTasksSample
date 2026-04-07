Use TeamTasksSample
GO


CREATE OR ALTER PROCEDURE TeamTasks.InsertTask
    @ProjectId INT,
    @Title NVARCHAR(100),
    @Description NVARCHAR(MAX),
    @AssigneeId INT,
    @Status NVARCHAR(20),
    @Priority NVARCHAR(10),
    @EstimatedComplexity INT,
    @DueDate DATE,
    @CompletionDate DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar existencia de ProjectId
    IF NOT EXISTS (SELECT 1 FROM TeamTasks.Projects WHERE ProjectId = @ProjectId)
    BEGIN
        RAISERROR('El ProjectId proporcionado no existe.', 16, 1);
        RETURN;
    END

    -- Validar existencia de AssigneeId
    IF NOT EXISTS (SELECT 1 FROM TeamTasks.Developers WHERE DeveloperId = @AssigneeId AND IsActive = 1)
    BEGIN
        RAISERROR('El AssigneeId proporcionado no existe o no está activo.', 16, 1);
        RETURN;
    END

    -- Insertar tarea
    INSERT INTO TeamTasks.Tasks (
        ProjectId, Title, Description, AssigneeId, Status, Priority, EstimatedComplexity, DueDate, CompletionDate, CreatedAt
    )
    VALUES (
        @ProjectId, @Title, @Description, @AssigneeId, @Status, @Priority, @EstimatedComplexity, @DueDate, @CompletionDate, GETDATE()
    );
END;
GO


