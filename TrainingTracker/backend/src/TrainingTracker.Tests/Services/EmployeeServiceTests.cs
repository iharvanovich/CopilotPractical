using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq;
using Xunit;
using TrainingTracker.Application.Interfaces;
using TrainingTracker.Application.Models.Employees;
using TrainingTracker.Application.Services;
using TrainingTracker.Domain.Entities;

namespace TrainingTracker.Tests.Services;

public class EmployeeServiceTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsMappedEmployees()
    {
        var repoMock = new Mock<IRepository<Employee>>();
        repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<string[]>()))
                .ReturnsAsync(new List<Employee>
                {
                    new Employee { FirstName = "John", LastName = "Doe", Email = "john@example.com" },
                    new Employee { FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" }
                });

        var service = new EmployeeService(repoMock.Object);

        var result = await service.GetAllAsync(CancellationToken.None);

        Assert.NotNull(result);
        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.Equal("John Doe", list[0].FullName);
        Assert.Equal("jane@example.com", list[1].Email);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsDetails_WhenFound()
    {
        var id = Guid.NewGuid();
        var repoMock = new Mock<IRepository<Employee>>();
        repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>(), It.IsAny<string[]>()))
                .ReturnsAsync(new Employee
                {
                    Id = id,
                    FirstName = "Anna",
                    LastName = "Bell",
                    Email = "anna@bell.com",
                    CourseAssignments = new List<CourseAssignment>
                    {
                        new CourseAssignment { Id = Guid.NewGuid(), CourseId = Guid.NewGuid(), Status = TrainingTracker.Domain.Enums.AssignmentStatus.Assigned }
                    }
                });

        var service = new EmployeeService(repoMock.Object);

        var details = await service.GetByIdAsync(id, CancellationToken.None);

        Assert.NotNull(details);
        Assert.Equal("Anna Bell", details!.FullName);
        Assert.Single(details.Assignments);
    }
}
