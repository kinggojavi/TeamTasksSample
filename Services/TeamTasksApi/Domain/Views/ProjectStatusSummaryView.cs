namespace TeamTasksApi.Domain.Views
{
    public class ProjectStatusSummaryView
    {
        public string ProjectName { get; set; }
        public string ClientName { get; set; }
        public int TotalTasks { get; set; }
        public int OpenTasks { get; set; }
        public int CompletedTasks { get; set; }
    }
}
