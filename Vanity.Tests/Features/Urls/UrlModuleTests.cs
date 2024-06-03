using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;
using System.Text.Json;
using Vanity.Web.Data;
using Vanity.Web.Models.Urls;

namespace Vanity.Tests.Features.Urls;

[Trait("Category", "Integration")]
public class UrlModuleTests : IClassFixture<ApiWebApplicationFactory<Program>>
{
    private readonly ApiWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public UrlModuleTests(ApiWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }

    [Fact]
    public async Task GetUrls_ReturnsOkResult()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            // Arrange
            var expectedEntiies = new List<UrlEntity>
            {
                new UrlEntity
                {
                    Alias = "foo",
                    LongUrl = "https://fobar.com/",
                },
                new UrlEntity
                {
                    Alias = "dnd-features",
                    LongUrl = "https://dnd.wizards.com/features",
                }
            };
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Urls.AddRange(expectedEntiies);
            context.SaveChanges();

            // Act
            var response = await _client.GetAsync("/api/urls");
            var body = await response.Content.ReadAsStringAsync();
            var actualEntities = JsonSerializer.Deserialize<List<UrlEntity>>(body);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedEntiies.Count, actualEntities?.Count);
        }
    }

    [Fact]
    public async Task GetUrlByAlias_ReturnsRedirectResult_WhenUrlExists()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            // Arrange
            UrlEntity expectedEntity = new()
            {
                Alias = "hello-world",
                LongUrl = "https://www.google.com.com/",
            };
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Urls.Add(expectedEntity);
            context.SaveChanges();

            // Act
            var response = await _client.GetAsync($"/{expectedEntity.Alias}");

            // Assert
            Assert.Equal(response.RequestMessage?.RequestUri?.ToString(), expectedEntity.LongUrl);
        }

    }

    [Fact]
    public async Task GetUrlByAlias_ReturnsNotFoundResult_WhenUrlDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/dnd-character-class-features");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateUrlWithAlias_ReturnsOkResult()
    {
        // Act
        UrlRequest request = new()
        {
            Alias = "Hello World",
            Url = "https://www.google.com.com/",
        };
        string JsonContent = JsonSerializer.Serialize(request);

        // Act
        var response = await _client.PostAsync("/api/urls", new StringContent(JsonContent, Encoding.UTF8, "application/json"));
        var body = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.EndsWith("hello-world\"", body);
    }

    [Fact]
    public async Task CreateUrlWithCode_ReturnsBadRequestResult_WhenUrlIsInvalid()
    {
        // Act
        UrlRequest request = new()
        {
            Url = "https://www.google.com.com/",
        };
        string JsonContent = JsonSerializer.Serialize(request);

        // Act
        var response = await _client.PostAsync("/api/urls", new StringContent(JsonContent, Encoding.UTF8, "application/json"));
        var body = await response.Content.ReadAsStringAsync();
        var lastSubstring = body.Split("/").Last();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(lastSubstring?.Count(), 8);
    }
}
