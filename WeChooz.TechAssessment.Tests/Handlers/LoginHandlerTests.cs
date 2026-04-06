using System.Security.Claims;
using Moq;
using WeChooz.TechAssessment.Application.Auth.Commands.Login;
using WeChooz.TechAssessment.Application.Interfaces.Authentication;

namespace WeChooz.TechAssessment.Tests.Handlers;

public sealed class LoginHandlerTests
{
    [Theory]
    [InlineData("formation")]
    [InlineData("sales")]
    public async Task HandleAsync_connexion_valide_appelle_SignIn_et_retourne_les_claims(string login)
    {
        var signIn = new Mock<IAuthenticationSignIn>();
        var handler = new LoginHandler(signIn.Object);
        var cmd = new LoginCommand(login);

        var result = await handler.HandleAsync(cmd, CancellationToken.None);

        Assert.Null(result.Failure);
        Assert.NotNull(result.Response);
        Assert.Equal(login, result.Response.Login);
        signIn.Verify(
            s => s.SignInAsync(It.Is<ClaimsPrincipal>(p => p.Identity!.Name == login), It.IsAny<CancellationToken>()),
            Times.Once);
        Assert.Contains(result.Response.Claims, c => c.Type == ClaimTypes.Role && c.Value == login);
        Assert.Contains(result.Response.Claims, c => c.Type == ClaimTypes.Name && c.Value == login);
    }

    [Fact]
    public async Task HandleAsync_login_invalide_ne_appelle_pas_SignIn()
    {
        var signIn = new Mock<IAuthenticationSignIn>();
        var handler = new LoginHandler(signIn.Object);

        var result = await handler.HandleAsync(new LoginCommand("intrus"), CancellationToken.None);

        Assert.Null(result.Response);
        Assert.Equal(LoginFailureKind.InvalidCredentials, result.Failure);
        signIn.Verify(s => s.SignInAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
