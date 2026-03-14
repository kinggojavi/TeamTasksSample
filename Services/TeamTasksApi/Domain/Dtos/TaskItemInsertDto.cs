using TeamTasksApi.Domain.Enums;
using TaskStatus = TeamTasksApi.Domain.Enums.TaskStatus;

namespace TeamTasksApi.Domain.Dtos
{
    public class TaskItemInsertDto
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssigneeId { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public int EstimatedComplexity { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}
