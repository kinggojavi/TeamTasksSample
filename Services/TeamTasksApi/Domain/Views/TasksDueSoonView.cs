namespace TeamTasksApi.Domain.Views
{
    public class TasksDueSoonView
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public string Assignee { get; set; }
        public string ProjectName { get; set; }
    }
}
