using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq;
using Xunit;
using TrainingTracker.Application.Interfaces;
using TrainingTracker.Application.Models.Courses;
using TrainingTracker.Application.Services;
using TrainingTracker.Domain.Entities;

namespace TrainingTracker.Tests.Services;

public class CourseServiceTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsMappedCourses()
    {
        var repoMock = new Mock<IRepository<Course>>();
        repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<string[]>()))
                .ReturnsAsync(new List<Course>
                {
                    new Course { Title = "C1", Description = "D1" },
                    new Course { Title = "C2", Description = "D2" }
                });

        var service = new CourseService(repoMock.Object);

        var result = await service.GetAllAsync(CancellationToken.None);

        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.Equal("C1", list[0].Title);
    }

    [Fact]
    public async Task CreateAsync_CreatesAndReturnsCourseModel()
    {
        var repoMock = new Mock<IRepository<Course>>();
        Course added = null!;
        repoMock.Setup(r => r.AddAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()))
                .Callback<Course, CancellationToken>((c, ct) => added = c)
                .Returns(Task.CompletedTask);
        repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var service = new CourseService(repoMock.Object);

        var create = new CreateCourseModel { Title = "New", Description = "Desc" };
        var created = await service.CreateAsync(create, CancellationToken.None);

        Assert.NotNull(created);
        Assert.Equal("New", created.Title);
        Assert.NotEqual(Guid.Empty, added.Id);
    }
}
