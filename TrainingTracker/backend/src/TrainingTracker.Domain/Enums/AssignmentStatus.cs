namespace TrainingTracker.Domain.Enums;

/// <summary>
/// Represents the lifecycle state of a course assignment for an employee.
/// Keep values simple so they map well to business rules and UI states.
/// </summary>
public enum AssignmentStatus
{
    Pending = 0,
    Assigned = 1,
    InProgress = 2,
    Completed = 3,
    Overdue = 4,
    Cancelled = 5
}
