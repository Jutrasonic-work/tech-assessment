using Moq;
using WeChooz.TechAssessment.Application.Courses.Commands.CreateCourse;
using WeChooz.TechAssessment.Application.Courses.Commands.DeleteCourse;
using WeChooz.TechAssessment.Application.Courses.Commands.UpdateCourse;
using WeChooz.TechAssessment.Application.Courses.Queries.GetCourseById;
using WeChooz.TechAssessment.Application.Courses.Queries.GetCourses;
using WeChooz.TechAssessment.Application.Persistence.Courses;
using WeChooz.TechAssessment.Domain.Common;
using WeChooz.TechAssessment.Domain.Courses;

namespace WeChooz.TechAssessment.Tests.Handlers;

public sealed class CourseHandlersTests
{
    [Fact]
    public async Task CreateCourseHandler_retourne_id_insere()
    {
        var repo = new Mock<ICourseRepository>();
        repo
            .Setup(r => r.InsertAsync(
                "Nom",
                "Court",
                "# Long",
                2,
                CseAudience.President,
                10,
                "Jean",
                "Dupont",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(42);
        var handler = new CreateCourseHandler(repo.Object);
        var cmd = new CreateCourseCommand("Nom", "Court", "# Long", 2, CseAudience.President, 10, "Jean", "Dupont");

        var result = await handler.HandleAsync(cmd, CancellationToken.None);

        Assert.Equal(42, result.CourseId);
    }

    [Fact]
    public async Task UpdateCourseHandler_retourne_updated_du_repo()
    {
        var repo = new Mock<ICourseRepository>();
        repo
            .Setup(r => r.UpdateAsync(5, "N", "S", "L", 1, CseAudience.DelegateElu, 8, "A", "B", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var handler = new UpdateCourseHandler(repo.Object);
        var cmd = new UpdateCourseCommand(5, "N", "S", "L", 1, CseAudience.DelegateElu, 8, "A", "B");

        var result = await handler.HandleAsync(cmd, CancellationToken.None);

        Assert.True(result.Updated);
    }

    [Fact]
    public async Task DeleteCourseHandler_delegue_au_repo()
    {
        var repo = new Mock<ICourseRepository>();
        var handler = new DeleteCourseHandler(repo.Object);

        await handler.HandleAsync(new DeleteCourseCommand(7), CancellationToken.None);

        repo.Verify(r => r.DeleteAsync(7, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetCourseByIdHandler_retourne_null_si_absent()
    {
        var repo = new Mock<ICourseRepository>();
        repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((Course?)null);
        var handler = new GetCourseByIdHandler(repo.Object);

        var result = await handler.HandleAsync(new GetCourseByIdQuery(1), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetCourseByIdHandler_mappe_le_cours()
    {
        var course = new Course(3, "C", "Short", "Long", 2, CseAudience.President, 5, new PersonName("F", "L"));
        var repo = new Mock<ICourseRepository>();
        repo.Setup(r => r.GetByIdAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(course);
        var handler = new GetCourseByIdHandler(repo.Object);

        var result = await handler.HandleAsync(new GetCourseByIdQuery(3), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(3, result.CourseId);
        Assert.Equal("C", result.Name);
        Assert.Equal("F", result.TrainerFirstName);
        Assert.Equal("L", result.TrainerLastName);
    }

    [Fact]
    public async Task GetCoursesHandler_mappe_la_liste()
    {
        var summaries = new List<CourseSummary>
        {
            new(1, "A", CseAudience.DelegateElu, 3, 12),
            new(2, "B", CseAudience.President, 1, 8),
        };
        var repo = new Mock<ICourseRepository>();
        repo.Setup(r => r.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(summaries);
        var handler = new GetCoursesHandler(repo.Object);

        var result = await handler.HandleAsync(new GetCoursesQuery(), CancellationToken.None);

        Assert.Equal(2, result.Items.Count);
        Assert.Equal("A", result.Items[0].Name);
        Assert.Equal(2, result.Items[1].CourseId);
    }
}
