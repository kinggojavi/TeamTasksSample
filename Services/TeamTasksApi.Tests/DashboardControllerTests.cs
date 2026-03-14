using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamTasksApi.Controllers;
using TeamTasksApi.Domain.Dtos;
using TeamTasksApi.Domain.Entities;
using TeamTasksApi.Domain.Enums;
using TeamTasksApi.Infrastructure.Persistence;
using Xunit;
using TaskStatus = TeamTasksApi.Domain.Enums.TaskStatus;

namespace TeamTasksApi.Tests
{
    public class DashboardControllerTests
    {

        private TeamTasksContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<TeamTasksContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // BD en memoria para pruebas
                .Options;

            return new TeamTasksContext(options);
        }

        [Fact]
        public async Task UpdateTaskStatus_ShouldUpdateStatusPriorityAndComplexity()
        {
            // Arrange
            var context = GetInMemoryContext();
            var controller = new DashboardController(context);

            var task = new TaskItem
            {
                TaskId = 1,
                ProjectId = 1,
                Title = "Test Task",
                Description = "Desc",
                AssigneeId = 1,
                Status = TaskStatus.ToDo,
                Priority = TaskPriority.Low,
                EstimatedComplexity = 3,
                DueDate = DateTime.Now.AddDays(5)
            };

            context.Tasks.Add(task);
            await context.SaveChangesAsync();

            var updateDto = new TaskUpdateDto
            {
                Status = TaskStatus.InProgress,
                Priority = TaskPriority.High,
                EstimatedComplexity = 8
            };

            // Act
            var result = await controller.UpdateTaskStatus(task.TaskId, updateDto);

            // Assert
            var updatedTask = await context.Tasks.FindAsync(task.TaskId);
            Assert.Equal(TaskStatus.InProgress, updatedTask.Status);
            Assert.Equal(TaskPriority.High, updatedTask.Priority);
            Assert.Equal(8, updatedTask.EstimatedComplexity);
        }


        [Fact]
        public async Task CreateTask_ShouldBuildParametersAndReturnOk()
        {
            // Arrange: contexto real con SQL Server
            var options = new DbContextOptionsBuilder<TeamTasksContext>()
                .UseSqlServer("Server=DESKTOP-IDRKO33\\SQLEXPRESS;Database=TeamTasksSample;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            using var context = new TeamTasksContext(options);
            var controller = new DashboardController(context);

            var dto = new TaskItemInsertDto
            {
                ProjectId = 1, // debe existir en la BD
                Title = "Nueva tarea 2",
                Description = "Descripción de prueba 2",
                AssigneeId = 2, // debe existir y estar activo en la BD
                Status = TaskStatus.ToDo,
                Priority = TaskPriority.High,
                EstimatedComplexity = 5,
                DueDate = DateTime.Now.AddDays(7),
                CompletionDate = null
            };

            // Act: invocar el método
            var result = await controller.CreateTask(dto);

            // Assert: verificar que devuelve Ok
            var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result);
            Assert.Contains("Task created successfully", okResult.Value.ToString());
        }
    }
}
