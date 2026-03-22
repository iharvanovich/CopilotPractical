namespace TrainingTracker.Application.Models.Dashboard;

public class DashboardSummaryModel
{
    public int TotalEmployees { get; set; }
    public int TotalCourses { get; set; }
    public int TotalAssignments { get; set; }

    public int AssignedCount { get; set; }
    public int InProgressCount { get; set; }
    public int CompletedCount { get; set; }
    public int OverdueCount { get; set; }
}
