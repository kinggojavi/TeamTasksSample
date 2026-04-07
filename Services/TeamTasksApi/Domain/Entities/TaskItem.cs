
namespace TeamTasksApi.Domain.Entities
{
    public class TaskItem
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int AssigneeId { get; set; }

        public string Status { get; set; } = "ToDo";
        public string Priority { get; set; } = "Medium";
        public int EstimatedComplexity { get; set; } = 1;

        public DateTime? DueDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual Project Project { get; set; }
        public virtual Developer Assignee { get; set; }
    }
}
