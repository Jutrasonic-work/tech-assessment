using Moq;
using WeChooz.TechAssessment.Application.Interfaces.Sessions;
using WeChooz.TechAssessment.Application.Sessions.Commands.CreateSession;
using WeChooz.TechAssessment.Application.Sessions.Commands.DeleteSession;
using WeChooz.TechAssessment.Application.Sessions.Commands.UpdateSession;
using WeChooz.TechAssessment.Application.Sessions.Queries.GetAdminSessionById;
using WeChooz.TechAssessment.Application.Sessions.Queries.GetAdminSessions;
using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Tests.Handlers;

public sealed class SessionHandlersTests
{
    private static readonly DateTime Start = new(2026, 6, 1, 9, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task CreateSessionHandler_retourne_id_insere()
    {
        // Arrange
        var repo = new Mock<ISessionRepository>();
        repo.Setup(r => r.InsertAsync(10, Start, SessionDeliveryMode.Remote, It.IsAny<CancellationToken>())).ReturnsAsync(99);
        var handler = new CreateSessionHandler(repo.Object);

        // Act
        var result = await handler.HandleAsync(new CreateSessionCommand(10, Start, SessionDeliveryMode.Remote), CancellationToken.None);

        // Assert
        Assert.Equal(99, result.SessionId);
    }

    [Fact]
    public async Task UpdateSessionHandler_retourne_updated_du_repo()
    {
        // Arrange
        var repo = new Mock<ISessionRepository>();
        repo.Setup(r => r.UpdateAsync(5, 2, Start, SessionDeliveryMode.InPerson, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var handler = new UpdateSessionHandler(repo.Object);

        // Act
        var result = await handler.HandleAsync(new UpdateSessionCommand(5, 2, Start, SessionDeliveryMode.InPerson), CancellationToken.None);

        // Assert
        Assert.True(result.Updated);
    }

    [Fact]
    public async Task DeleteSessionHandler_delegue_au_repo()
    {
        // Arrange
        var repo = new Mock<ISessionRepository>();
        var handler = new DeleteSessionHandler(repo.Object);

        // Act
        await handler.HandleAsync(new DeleteSessionCommand(3), CancellationToken.None);

        // Assert
        repo.Verify(r => r.DeleteAsync(3, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAdminSessionByIdHandler_retourne_null_si_absent()
    {
        // Arrange
        var repo = new Mock<ISessionRepository>();
        repo.Setup(r => r.GetAdminByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((AdminSessionOverview?)null);
        var handler = new GetAdminSessionByIdHandler(repo.Object);

        // Act
        var result = await handler.HandleAsync(new GetAdminSessionByIdQuery(1), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAdminSessionByIdHandler_mappe_la_session()
    {
        // Arrange
        var row = new AdminSessionOverview(4, 8, "Cours", Start, SessionDeliveryMode.InPerson, 3, 20);
        var repo = new Mock<ISessionRepository>();
        repo.Setup(r => r.GetAdminByIdAsync(4, It.IsAny<CancellationToken>())).ReturnsAsync(row);
        var handler = new GetAdminSessionByIdHandler(repo.Object);

        // Act
        var result = await handler.HandleAsync(new GetAdminSessionByIdQuery(4), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.SessionId);
        Assert.Equal("Cours", result.CourseName);
        Assert.Equal(3, result.ParticipantCount);
    }

    [Fact]
    public async Task GetAdminSessionsHandler_mappe_la_liste()
    {
        // Arrange
        var rows = new List<AdminSessionOverview>
        {
            new(1, 1, "X", Start, SessionDeliveryMode.Remote, 0, 10),
        };
        var repo = new Mock<ISessionRepository>();
        repo.Setup(r => r.ListAdminAsync(It.IsAny<CancellationToken>())).ReturnsAsync(rows);
        var handler = new GetAdminSessionsHandler(repo.Object);

        // Act
        var result = await handler.HandleAsync(new GetAdminSessionsQuery(), CancellationToken.None);

        // Assert
        Assert.Single(result.Items);
        Assert.Equal("X", result.Items[0].CourseName);
    }
}
