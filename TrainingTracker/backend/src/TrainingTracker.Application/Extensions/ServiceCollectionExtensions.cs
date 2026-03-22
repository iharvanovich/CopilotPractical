using Microsoft.Extensions.DependencyInjection;
using TrainingTracker.Application.Interfaces;
using TrainingTracker.Application.Services;

namespace TrainingTracker.Application.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers application services (business logic) into DI.
    /// Infrastructure must register repositories before calling this in the composition root.
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ICourseCategoryService, CourseCategoryService>();
        services.AddScoped<ICourseAssignmentService, CourseAssignmentService>();
        services.AddScoped<IDashboardService, DashboardService>();

        return services;
    }
}
