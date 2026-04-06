using Markdig;
using Shared.Mediator;
using WeChooz.TechAssessment.Application.Interfaces.Sessions;

namespace WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessionDetail;

public sealed class GetPublicSessionDetailHandler(ISessionRepository sessions) : IRequestHandler<GetPublicSessionDetailQuery, GetPublicSessionDetailResponse?>
{
    private static readonly MarkdownPipeline MarkdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

    public async Task<GetPublicSessionDetailResponse?> HandleAsync(GetPublicSessionDetailQuery request, CancellationToken cancellationToken = default)
    {
        var detail = await sessions.GetPublicDetailAsync(request.SessionId, cancellationToken);
        if (detail is null)
        {
            return null;
        }

        var html = string.IsNullOrWhiteSpace(detail.LongDescriptionMarkdown)
            ? string.Empty
            : Markdown.ToHtml(detail.LongDescriptionMarkdown, MarkdownPipeline);

        return new GetPublicSessionDetailResponse(
            detail.SessionId,
            detail.CourseName,
            detail.ShortDescription,
            html,
            detail.CseAudience,
            detail.StartDate,
            detail.DurationDays,
            detail.DeliveryMode,
            detail.RemainingSeats,
            detail.TrainerFirstName,
            detail.TrainerLastName);
    }
}
