using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq;
using Xunit;
using TrainingTracker.Application.Interfaces;
using TrainingTracker.Application.Models.Assignments;
using TrainingTracker.Application.Services;
using TrainingTracker.Domain.Entities;
using TrainingTracker.Domain.Enums;

namespace TrainingTracker.Tests.Services;

public class CourseAssignmentServiceTests
{
    [Fact]
    public async Task GetAllAsync_MarksOverdueInMemory()
    {
        var repoMock = new Mock<IRepository<CourseAssignment>>();
        var now = DateTime.UtcNow;
        repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<string[]>()))
                .ReturnsAsync(new List<CourseAssignment>
                {
                    new CourseAssignment { Id = Guid.NewGuid(), AssignedAt = now.AddDays(-10), DueAt = now.AddDays(-1), Status = AssignmentStatus.Assigned },
                    new CourseAssignment { Id = Guid.NewGuid(), AssignedAt = now, DueAt = now.AddDays(5), Status = AssignmentStatus.Assigned }
                });

        var service = new CourseAssignmentService(repoMock.Object, Mock.Of<IRepository<Employee>>(), Mock.Of<IRepository<Course>>());

        var list = (await service.GetAllAsync(CancellationToken.None)).ToList();

        Assert.Equal(2, list.Count);
        Assert.Equal(AssignmentStatus.Overdue, list[0].Status);
        Assert.NotEqual(AssignmentStatus.Overdue, list[1].Status);
    }

    [Fact]
    public async Task CreateAsync_ThrowsWhenEmployeeOrCourseMissing()
    {
        var repoMock = new Mock<IRepository<CourseAssignment>>();
        var empRepo = new Mock<IRepository<Employee>>();
        var courseRepo = new Mock<IRepository<Course>>();

        empRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((Employee?)null);
        courseRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync((Course?)null);

        var service = new CourseAssignmentService(repoMock.Object, empRepo.Object, courseRepo.Object);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.CreateAsync(new CreateAssignmentModel { EmployeeId = Guid.NewGuid(), CourseId = Guid.NewGuid() }, CancellationToken.None));
    }
}
