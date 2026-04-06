using Moq;
using WeChooz.TechAssessment.Application.Interfaces.Sessions;
using WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessionDetail;
using WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessions;
using WeChooz.TechAssessment.Domain.Courses;
using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Tests.Handlers;

public sealed class PublicSessionHandlersTests
{
    private static readonly DateTime Start = new(2026, 7, 15, 14, 0, 0, DateTimeKind.Utc);

    [Fact]
    public async Task GetPublicSessionsHandler_passe_les_criteres_au_repo()
    {
        // Arrange
        var repo = new Mock<ISessionRepository>();
        repo.Setup(r => r.ListPublicAsync(It.IsAny<PublicSessionListCriteria>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<PublicSessionListing>());
        var handler = new GetPublicSessionsHandler(repo.Object);
        var query = new GetPublicSessionsQuery(
            CseAudience.President,
            SessionDeliveryMode.InPerson,
            Start,
            Start.AddDays(1),
            null,
            null);

        // Act
        await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        repo.Verify(
            r => r.ListPublicAsync(
                It.Is<PublicSessionListCriteria>(c =>
                    c.Audience == CseAudience.President
                    && c.DeliveryMode == SessionDeliveryMode.InPerson
                    && c.StartAfter == Start
                    && c.StartBefore == Start.AddDays(1)
                    && c.StartFrom == null
                    && c.StartTo == null),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetPublicSessionsHandler_mappe_les_lignes()
    {
        // Arrange
        var listing = new PublicSessionListing(
            1,
            "Formation",
            "Résumé",
            CseAudience.DelegateElu,
            Start,
            2,
            SessionDeliveryMode.Remote,
            4,
            "Jean",
            "Martin");
        var repo = new Mock<ISessionRepository>();
        repo.Setup(r => r.ListPublicAsync(It.IsAny<PublicSessionListCriteria>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { listing });
        var handler = new GetPublicSessionsHandler(repo.Object);

        // Act
        var result = await handler.HandleAsync(
            new GetPublicSessionsQuery(null, null, null, null, null, null),
            CancellationToken.None);

        // Assert
        Assert.Single(result.Items);
        Assert.Equal("Formation", result.Items[0].CourseName);
        Assert.Equal(4, result.Items[0].RemainingSeats);
    }

    [Fact]
    public async Task GetPublicSessionDetailHandler_retourne_null_si_absent()
    {
        // Arrange
        var repo = new Mock<ISessionRepository>();
        repo.Setup(r => r.GetPublicDetailAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((PublicSessionCatalogDetail?)null);
        var handler = new GetPublicSessionDetailHandler(repo.Object);

        // Act
        var result = await handler.HandleAsync(new GetPublicSessionDetailQuery(1), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetPublicSessionDetailHandler_markdown_vide_donne_html_vide()
    {
        // Arrange
        var detail = new PublicSessionCatalogDetail(
            2,
            "C",
            "S",
            "   ",
            CseAudience.President,
            Start,
            1,
            SessionDeliveryMode.InPerson,
            2,
            "A",
            "B");
        var repo = new Mock<ISessionRepository>();
        repo.Setup(r => r.GetPublicDetailAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(detail);
        var handler = new GetPublicSessionDetailHandler(repo.Object);

        // Act
        var result = await handler.HandleAsync(new GetPublicSessionDetailQuery(2), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.LongDescriptionHtml);
    }

    [Fact]
    public async Task GetPublicSessionDetailHandler_convertit_le_markdown_en_html()
    {
        // Arrange
        var detail = new PublicSessionCatalogDetail(
            3,
            "Titre",
            "Court",
            "# Hello",
            CseAudience.DelegateElu,
            Start,
            3,
            SessionDeliveryMode.Remote,
            1,
            "F",
            "L");
        var repo = new Mock<ISessionRepository>();
        repo.Setup(r => r.GetPublicDetailAsync(3, It.IsAny<CancellationToken>())).ReturnsAsync(detail);
        var handler = new GetPublicSessionDetailHandler(repo.Object);

        // Act
        var result = await handler.HandleAsync(new GetPublicSessionDetailQuery(3), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Hello", result.LongDescriptionHtml, StringComparison.Ordinal);
        Assert.Contains("<h1", result.LongDescriptionHtml, StringComparison.OrdinalIgnoreCase);
        Assert.Equal("Titre", result.CourseName);
        Assert.Equal(1, result.RemainingSeats);
    }
}
