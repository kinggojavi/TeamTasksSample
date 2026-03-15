
namespace TeamTasksApi.Domain.Dtos
{
    public class TaskUpdateDto
    {
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public int? EstimatedComplexity { get; set; }
    }
}
