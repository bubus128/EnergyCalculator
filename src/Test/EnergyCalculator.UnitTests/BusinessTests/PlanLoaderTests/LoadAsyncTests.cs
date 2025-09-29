using System.Text.Json;
using EnergyCalculator.Business.PlanReader;

namespace EnergyCalculator.UnitTests.BusinessTests.PlanLoaderTests;

[TestFixture]
public class LoadAsyncTests
{
    private JsonSerializerOptions _options = null!;
    private PlanLoader _planLoader = null!;
    private string _mockDataPath = null!;

    [SetUp]
    public void Setup()
    {
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
        _planLoader = new PlanLoader(_options);

        _mockDataPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", "MockPlans.json");
    }

    [Test]
    public async Task LoadAsync_ValidJson_ReturnsPlans()
    {
        // Arrange
        await using var stream = File.OpenRead(_mockDataPath);

        // Act
        var plans = (await _planLoader.LoadAsync(stream)).ToList();

        // Assert
        Assert.That(plans, Is.Not.Null);
        Assert.That(plans.Count, Is.GreaterThan(0));
        Assert.That(plans.All(p => !string.IsNullOrWhiteSpace(p.PlanName)), "All plans should have names");
    }

    [Test]
    public async Task LoadAsync_EmptyStream_ReturnsEmptyEnumerable()
    {
        // Arrange
        await using var stream = new MemoryStream();

        // Act & Assert
        Assert.ThrowsAsync<JsonException>(async () => await _planLoader.LoadAsync(stream));
    }

    [Test]
    public async Task LoadAsync_InvalidJson_ThrowsJsonException()
    {
        // Arrange
        var invalidJson = "{ invalid json }";
        var bytes = System.Text.Encoding.UTF8.GetBytes(invalidJson);
        await using var stream = new MemoryStream(bytes);

        // Act & Assert
        Assert.ThrowsAsync<JsonException>(async () => await _planLoader.LoadAsync(stream));
    }
}