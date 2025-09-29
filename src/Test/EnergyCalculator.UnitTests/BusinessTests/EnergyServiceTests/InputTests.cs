using System.Text.Json;
using EnergyCalculator.Business.EnergyService;
using EnergyCalculator.Data.Models;
using EnergyCalculator.Data.Repositories;
using FakeItEasy;

namespace EnergyCalculator.UnitTests.BusinessTests.EnergyServiceTests;

[TestFixture]
public class InputTests
{
    private IEnergyPlanRepository _repository;
    private EnergyService _service;
    private string _mockDataPath;
    
    [SetUp]
    public void Setup()
    {
        _repository = A.Fake<IEnergyPlanRepository>();
        _service = new EnergyService(_repository);
        
        _mockDataPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", "MockPlans.json");
    }
    
    [Test]
    public void Input_FileDoesNotExist_ThrowsFileNotFoundException()
    {
        // Arrange
        string invalidPath = "nonexistent.json";
        
        // Act
        var ex = Assert.ThrowsAsync<FileNotFoundException>(async () => await _service.Input(invalidPath));
        
        // Assert
        Assert.That(ex.Message, Does.Contain("Energy plan file not found"));
    }

    [Test]
    public void Input_InvalidExtension_ThrowsArgumentException()
    {
        // Arrange
        string invalidPath = Path.ChangeExtension(_mockDataPath, ".txt");
        File.Copy(_mockDataPath, invalidPath, true);
        
        // Act
        var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _service.Input(invalidPath));
        
        // Assert
        Assert.That(ex.Message, Does.Contain("Invalid file type"));
        
        // Cleanup
        File.Delete(invalidPath);
    }

    [Test]
    public async Task Input_ValidJson_CallsAddPlanForEachPlan()
    {
        // Arrange
        var json = await File.ReadAllTextAsync(_mockDataPath);
        var plans = JsonSerializer.Deserialize<List<EnergyPlan>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        });

        // Act
        await _service.Input(_mockDataPath);

        // Assert
        A.CallTo(() => _repository.AddPlan(A<EnergyPlan>.Ignored))
            .MustHaveHappened(plans!.Count, Times.Exactly);
    }
}