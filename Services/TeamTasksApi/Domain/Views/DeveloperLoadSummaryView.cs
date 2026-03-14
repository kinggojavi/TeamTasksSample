namespace TeamTasksApi.Domain.Views
{
    public class DeveloperLoadSummaryView
    {
        public string DeveloperName { get; set; }
        public int OpenTasksCount { get; set; }
        public double AverageEstimatedComplexity { get; set; }
    }
}
