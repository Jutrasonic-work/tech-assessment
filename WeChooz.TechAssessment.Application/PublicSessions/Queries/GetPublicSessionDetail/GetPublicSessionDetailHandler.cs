using Markdig;
using Shared.Mediator.Application;
using WeChooz.TechAssessment.Domain.Repositories;

namespace WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessionDetail;

public sealed class GetPublicSessionDetailHandler(ISessionRepository sessions) : IRequestHandler<GetPublicSessionDetailQuery, GetPublicSessionDetailResponse?>
{
    private static readonly MarkdownPipeline MarkdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

    public async Task<GetPublicSessionDetailResponse?> HandleAsync(GetPublicSessionDetailQuery request, CancellationToken cancellationToken = default)
    {
        var row = await sessions.GetPublicDetailAsync(request.SessionId, cancellationToken);
        if (row is null)
        {
            return null;
        }

        var html = string.IsNullOrWhiteSpace(row.LongDescriptionMarkdown)
            ? string.Empty
            : Markdown.ToHtml(row.LongDescriptionMarkdown, MarkdownPipeline);

        return new GetPublicSessionDetailResponse(
            row.SessionId,
            row.CourseName,
            row.ShortDescription,
            html,
            row.CseAudience,
            row.StartDate,
            row.DurationDays,
            row.DeliveryMode,
            row.RemainingSeats,
            row.TrainerFirstName,
            row.TrainerLastName);
    }
}
