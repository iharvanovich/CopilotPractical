using TrainingTracker.Application.Models.Dashboard;

namespace TrainingTracker.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardSummaryModel> GetSummaryAsync(CancellationToken cancellationToken);
}
