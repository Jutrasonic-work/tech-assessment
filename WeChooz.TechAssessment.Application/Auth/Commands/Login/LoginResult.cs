namespace WeChooz.TechAssessment.Application.Auth.Commands.Login;

public sealed record LoginResult(LoginResponse? Response, LoginFailureKind? Failure);

public enum LoginFailureKind
{
    InvalidCredentials,
}

public sealed record LoginResponse(string Login, IReadOnlyList<LoginClaimDto> Claims);

public sealed record LoginClaimDto(string Type, string Value);
