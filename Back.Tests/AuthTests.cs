using Xunit;
using FluentAssertions;
using Back.Application.DTO;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

public class AuthTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithCorrectCredentials_ReturnsToken()
    {
        // Arrange
        var loginInfo = new LoginDTO { Username = "admin", Password = "123456" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginInfo);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<dynamic>();
        // Verificamos que el token no sea nulo (Punto 5 y 6)
        Assert.NotNull(content?.GetProperty("token").GetString());
    }
}