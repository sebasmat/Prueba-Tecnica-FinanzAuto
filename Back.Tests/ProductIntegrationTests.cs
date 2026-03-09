using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using Back.Application.DTO;


public class ProductIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProducts_ReturnsSuccessAndCorrectPagination(){
        // 1. Loguearse para obtener el token
        var loginInfo = new LoginDTO { Username = "admin", Password = "123456" };
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginInfo);
        loginResponse.EnsureSuccessStatusCode();

        // 2. Leer el token directamente como string (ajusta "token" si tu API devuelve el nombre en mayúscula "Token")
        var result = await loginResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        string token = result!["token"]; // Si en tu JSON sale "token", usa minúscula. Si sale "Token", usa mayúscula.

        // 2. Agregar el token a la cabecera "Authorization"
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // 3. Ahora sí: Pedir los productos
        var response = await _client.GetAsync("/api/product?page=1&pageSize=10");

        // Assert
        response.EnsureSuccessStatusCode(); // ¡Ahora ya no debería dar 401!
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("\"pageSize\":10"); 
    }
}