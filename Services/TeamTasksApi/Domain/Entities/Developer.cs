namespace TeamTasksApi.Domain.Entities
{
    public class Developer
    {
        public int DeveloperId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string FullName => $"{FirstName} {LastName}";

        // Relación con tareas
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
