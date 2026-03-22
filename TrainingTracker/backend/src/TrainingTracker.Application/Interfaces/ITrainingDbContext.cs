using TrainingTracker.Domain.Entities;

namespace TrainingTracker.Application.Interfaces;

public interface ITrainingDbContext
{
    // Employees
    Task<List<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken);
    Task<Employee?> GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken);
    Task UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken);
    Task DeleteEmployeeAsync(Employee employee, CancellationToken cancellationToken);

    // Courses
    Task<List<Course>> GetAllCoursesAsync(CancellationToken cancellationToken);
    Task<Course?> GetCourseByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddCourseAsync(Course course, CancellationToken cancellationToken);
    Task UpdateCourseAsync(Course course, CancellationToken cancellationToken);
    Task DeleteCourseAsync(Course course, CancellationToken cancellationToken);

    // Categories
    Task<List<CourseCategory>> GetAllCategoriesAsync(CancellationToken cancellationToken);
    Task<CourseCategory?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddCategoryAsync(CourseCategory category, CancellationToken cancellationToken);
    Task UpdateCategoryAsync(CourseCategory category, CancellationToken cancellationToken);
    Task DeleteCategoryAsync(CourseCategory category, CancellationToken cancellationToken);

    // Assignments
    Task<List<CourseAssignment>> GetAllAssignmentsAsync(CancellationToken cancellationToken);
    Task<CourseAssignment?> GetAssignmentByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAssignmentAsync(CourseAssignment assignment, CancellationToken cancellationToken);
    Task UpdateAssignmentAsync(CourseAssignment assignment, CancellationToken cancellationToken);
    Task DeleteAssignmentAsync(CourseAssignment assignment, CancellationToken cancellationToken);

    // Save
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
