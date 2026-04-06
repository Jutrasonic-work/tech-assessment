using Moq;
using WeChooz.TechAssessment.Application.Participants.Commands.AddParticipant;
using WeChooz.TechAssessment.Application.Participants.Commands.RemoveParticipant;
using WeChooz.TechAssessment.Application.Participants.Commands.UpdateParticipant;
using WeChooz.TechAssessment.Application.Participants.Queries.GetParticipantsBySession;
using WeChooz.TechAssessment.Application.Interfaces.Participants;
using WeChooz.TechAssessment.Domain.Common;
using WeChooz.TechAssessment.Domain.Participants;

namespace WeChooz.TechAssessment.Tests.Handlers;

public sealed class ParticipantHandlersTests
{
    [Fact]
    public async Task AddParticipantHandler_retourne_id_insere()
    {
        var repo = new Mock<IParticipantRepository>();
        repo
            .Setup(r => r.InsertAsync(9, "Martin", "Paul", "p@x.com", "WeChooz", It.IsAny<CancellationToken>()))
            .ReturnsAsync(100);
        var handler = new AddParticipantHandler(repo.Object);

        var result = await handler.HandleAsync(
            new AddParticipantCommand(9, "Martin", "Paul", "p@x.com", "WeChooz"),
            CancellationToken.None);

        Assert.Equal(100, result.ParticipantId);
    }

    [Fact]
    public async Task UpdateParticipantHandler_retourne_updated_du_repo()
    {
        var repo = new Mock<IParticipantRepository>();
        repo
            .Setup(r => r.UpdateAsync(2, 9, "M", "P", "e@e.com", "Co", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var handler = new UpdateParticipantHandler(repo.Object);

        var result = await handler.HandleAsync(
            new UpdateParticipantCommand(2, 9, "M", "P", "e@e.com", "Co"),
            CancellationToken.None);

        Assert.True(result.Updated);
    }

    [Fact]
    public async Task RemoveParticipantHandler_delegue_au_repo()
    {
        var repo = new Mock<IParticipantRepository>();
        var handler = new RemoveParticipantHandler(repo.Object);

        await handler.HandleAsync(new RemoveParticipantCommand(11), CancellationToken.None);

        repo.Verify(r => r.DeleteAsync(11, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetParticipantsBySessionHandler_mappe_la_liste()
    {
        var participants = new List<Participant>
        {
            new(1, 5, new PersonName("Alice", "Durand"), new EmailAddress("a@b.co"), "Entreprise"),
        };
        var repo = new Mock<IParticipantRepository>();
        repo.Setup(r => r.ListBySessionAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(participants);
        var handler = new GetParticipantsBySessionHandler(repo.Object);

        var result = await handler.HandleAsync(new GetParticipantsBySessionQuery(5), CancellationToken.None);

        Assert.Single(result.Items);
        Assert.Equal("Durand", result.Items[0].LastName);
        Assert.Equal("Alice", result.Items[0].FirstName);
        Assert.Equal("a@b.co", result.Items[0].Email);
    }
}
