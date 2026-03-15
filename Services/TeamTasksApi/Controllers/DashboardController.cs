using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TeamTasksApi.Domain.Dtos;
using TeamTasksApi.Domain.Entities;
using TeamTasksApi.Infrastructure.Persistence;


namespace TeamTasksApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class DashboardController : ControllerBase
    {
        private readonly TeamTasksContext _context;

        public DashboardController(TeamTasksContext context)
        {
            _context = context;
        }

        // 1. Obtener todos los proyectos
        [HttpGet("projects")]
        public async Task<IActionResult> GetProjects()
        {
            var data = await _context.Projects.ToListAsync();
            return Ok(data);
        }

        // 2. Obtener tareas de un proyecto específico con filtros opcionales
        [HttpGet("projects/{id}/tasks")]
        public async Task<IActionResult> GetProjectTasks(
            int id,
            int page = 1,
            int pageSize = 5,
            string? state = null,
            int? developer = null)
        {
            // Construir la consulta base
            IQueryable<TaskItem> query = _context.Tasks
                .Include(t => t.Assignee)
                .Where(t => t.ProjectId == id);

            // Filtro opcional por estado
            if (!string.IsNullOrEmpty(state))
            {
                query = query.Where(t => t.Status == state);
            }

            // Filtro opcional por desarrollador
            if (developer.HasValue)
            {
                query = query.Where(t => t.AssigneeId == developer.Value);
            }

            // Proyección y paginación
            var data = await query
                .OrderBy(t => t.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new
                {
                    d.TaskId,
                    d.ProjectId,
                    d.Title,
                    d.Description,
                    d.AssigneeId,
                    d.Status,
                    d.Priority,
                    d.EstimatedComplexity,
                    d.DueDate,
                    d.CompletionDate,
                    d.CreatedAt,
                    AssignedTo = d.Assignee.FullName
                })
                .ToListAsync();

            return Ok(data);
        }



        // 3. Vista: carga de trabajo por desarrollador
        [HttpGet("developer-load-summary")]
        public async Task<IActionResult> GetDeveloperLoadSummary()
        {
            var data = await _context.DeveloperLoadSummary
                .OrderBy(d => d.DeveloperName)                // primero ordena por nombre
                .ThenBy(d => d.AverageEstimatedComplexity)    // luego por promedio de complejidad
                .ToListAsync();

            return Ok(data);
        }

        // 4. Vista: estado de proyectos
        [HttpGet("project-status-summary")]
        public async Task<IActionResult> GetProjectStatusSummary()
        {
            var data = await _context.ProjectStatusSummary
                .OrderBy(p => p.ProjectName)
                .ToListAsync();
            return Ok(data);
        }

        // 5. Vista: tareas próximas a vencer
        [HttpGet("tasks-due-soon")]
        public async Task<IActionResult> GetTasksDueSoon()
        {
            var data = await _context.TasksDueSoon.ToListAsync();
            return Ok(data);
        }

        // 6. Vista: riesgo de retraso por desarrollador
        [HttpGet("developer-risk-summary")]
        public async Task<IActionResult> GetDeveloperRiskSummary()
        {
            var data = await _context.DeveloperRiskSummary
                .OrderBy(p => p.DeveloperName)
                .ToListAsync();
            return Ok(data);
        }

        // 7. Obtener desarrolladores activos
        [HttpGet("developers")]
        public async Task<IActionResult> GetActiveDevelopers()
        {
            var data = await _context.Developers
                .Where(d => d.IsActive)
                .Select(d => new
                {
                    d.DeveloperId,
                    FullName = d.FirstName + " " + d.LastName,
                    d.Email
                })
                .ToListAsync();

            return Ok(data);
        }

        // 8. Actualizar estado de una tarea
        [HttpPut("tasks/{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] TaskUpdateDto update)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound($"Task with id {id} not found.");
            }

            // Actualizar campos si vienen en el DTO
            if (update.Status is not null)
                task.Status = update.Status;

            if (update.Priority is not null)
                task.Priority = update.Priority;

            if (update.EstimatedComplexity.HasValue)
                task.EstimatedComplexity = update.EstimatedComplexity.Value;

            await _context.SaveChangesAsync();

            return Ok(task);
        }

        // Crear una nueva tarea usando procedimiento almacenado
        [HttpPost("tasks")]
        public async Task<IActionResult> CreateTask([FromBody] TaskItemInsertDto newTask)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@ProjectId", newTask.ProjectId),
                    new SqlParameter("@Title", newTask.Title ?? (object)DBNull.Value),
                    new SqlParameter("@Description", newTask.Description ?? (object)DBNull.Value),
                    new SqlParameter("@AssigneeId", newTask.AssigneeId),
                    new SqlParameter("@Status", newTask.Status),    
                    new SqlParameter("@Priority", newTask.Priority),
                    new SqlParameter("@EstimatedComplexity", newTask.EstimatedComplexity),
                    new SqlParameter("@DueDate", newTask.DueDate),
                    new SqlParameter("@CompletionDate", newTask.CompletionDate ?? (object)DBNull.Value)
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC TeamTasks.InsertTask @ProjectId, @Title, @Description, @AssigneeId, @Status, @Priority, @EstimatedComplexity, @DueDate, @CompletionDate",
                    parameters
                );

                return Ok(new { message = "Task created successfully via stored procedure." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}