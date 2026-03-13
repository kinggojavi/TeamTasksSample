
Use TeamTasksSample
GO

CREATE OR ALTER VIEW TeamTasks.vw_DeveloperLoadSummary
AS
SELECT 
    d.FirstName + ' ' + d.LastName AS DeveloperName,
    COUNT(t.TaskId) AS OpenTasksCount,
    AVG(t.EstimatedComplexity) AS AverageEstimatedComplexity
FROM TeamTasks.Developers d
LEFT JOIN TeamTasks.Tasks t 
    ON d.DeveloperId = t.AssigneeId 
    AND t.Status <> 'Completed'
WHERE d.IsActive = 1
GROUP BY d.FirstName, d.LastName;
GO



CREATE OR ALTER VIEW TeamTasks.vw_ProjectStatusSummary
AS
SELECT 
    p.Name AS ProjectName,
    COUNT(t.TaskId) AS TotalTasks,
    SUM(CASE WHEN t.Status <> 'Completed' THEN 1 ELSE 0 END) AS OpenTasks,
    SUM(CASE WHEN t.Status = 'Completed' THEN 1 ELSE 0 END) AS CompletedTasks
FROM TeamTasks.Projects p
LEFT JOIN TeamTasks.Tasks t 
    ON p.ProjectId = t.ProjectId
GROUP BY p.Name;
GO


CREATE OR ALTER VIEW TeamTasks.vw_TasksDueSoon
AS
SELECT 
    t.TaskId,
    t.Title,
    t.DueDate,
    d.FirstName + ' ' + d.LastName AS Assignee,
    p.Name AS ProjectName
FROM TeamTasks.Tasks t
INNER JOIN TeamTasks.Developers d ON t.AssigneeId = d.DeveloperId
INNER JOIN TeamTasks.Projects p ON t.ProjectId = p.ProjectId
WHERE t.Status <> 'Completed'
  AND t.DueDate BETWEEN GETDATE() AND DATEADD(DAY, 7, GETDATE());
GO


CREATE OR ALTER VIEW TeamTasks.vw_DeveloperRiskSummary
AS
WITH CompletedDelays AS (
    SELECT 
        AssigneeId,
        CASE 
            WHEN CompletionDate IS NULL THEN 0
            WHEN CompletionDate <= DueDate THEN 0
            ELSE DATEDIFF(DAY, DueDate, CompletionDate)
        END AS DelayDays
    FROM TeamTasks.Tasks
    WHERE Status = 'Completed'
),
OpenTasks AS (
    SELECT 
        AssigneeId,
        DueDate
    FROM TeamTasks.Tasks
    WHERE Status <> 'Completed'
)
SELECT 
    d.FirstName + ' ' + d.LastName AS DeveloperName,
    COUNT(o.DueDate) AS OpenTasksCount,
    ISNULL(AVG(c.DelayDays),0) AS AvgDelayDays,
    MIN(o.DueDate) AS NearestDueDate,
    MAX(o.DueDate) AS LatestDueDate,
    DATEADD(DAY, ISNULL(AVG(c.DelayDays),0), MAX(o.DueDate)) AS PredictedCompletionDate,
    CASE 
        WHEN DATEADD(DAY, ISNULL(AVG(c.DelayDays),0), MAX(o.DueDate)) > MAX(o.DueDate) 
             OR ISNULL(AVG(c.DelayDays),0) > 5 THEN 1
        ELSE 0
    END AS HighRiskFlag
FROM TeamTasks.Developers d
LEFT JOIN CompletedDelays c ON d.DeveloperId = c.AssigneeId
LEFT JOIN OpenTasks o ON d.DeveloperId = o.AssigneeId
WHERE d.IsActive = 1
GROUP BY d.FirstName, d.LastName;
GO
