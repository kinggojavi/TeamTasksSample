using TeamTasksApi.Domain.Enums;
using TaskStatus = TeamTasksApi.Domain.Enums.TaskStatus;

namespace TeamTasksApi.Domain.Dtos
{
    public class TaskUpdateDto
    {
        public TaskStatus? Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public int? EstimatedComplexity { get; set; }
    }
}
