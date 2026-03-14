using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TeamTasksApi.Domain.Dtos;
using TeamTasksApi.Infrastructure.Persistence;


namespace TeamTasksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        // 2. Obtener tareas de un proyecto específico
        [HttpGet("projects/{id}/tasks")]
        public async Task<IActionResult> GetProjectTasks(int id, int page = 1, int pageSize = 5)
        {
            var data = await _context.Tasks
                .Where(t => t.ProjectId == id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return Ok(data);
        }

        // 3. Vista: carga de trabajo por desarrollador
        [HttpGet("developer-load-summary")]
        public async Task<IActionResult> GetDeveloperLoadSummary()
        {
            var data = await _context.DeveloperLoadSummary.ToListAsync();
            return Ok(data);
        }

        // 4. Vista: estado de proyectos
        [HttpGet("project-status-summary")]
        public async Task<IActionResult> GetProjectStatusSummary()
        {
            var data = await _context.ProjectStatusSummary.ToListAsync();
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
            var data = await _context.DeveloperRiskSummary.ToListAsync();
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
            if (update.Status.HasValue)
                task.Status = update.Status.Value;

            if (update.Priority.HasValue)
                task.Priority = update.Priority.Value;

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
                    new SqlParameter("@Status", newTask.Status.ToString()),     // Enum → NVARCHAR
                    new SqlParameter("@Priority", newTask.Priority.ToString()), // Enum → NVARCHAR
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