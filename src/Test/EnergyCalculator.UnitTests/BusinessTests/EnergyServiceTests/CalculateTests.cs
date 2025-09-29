using System.Text.Json;
using EnergyCalculator.Business.EnergyService;
using EnergyCalculator.Data.Models;
using EnergyCalculator.Data.Repositories;
using FakeItEasy;

namespace EnergyCalculator.UnitTests.BusinessTests.EnergyServiceTests;

[TestFixture]
public class CalculateTests
{

    private IEnergyPlanRepository _repository;
    private EnergyService _service;
    private IReadOnlyList<EnergyPlan>? _plans;

    [SetUp]
    public void Setup()
    {
        _repository = A.Fake<IEnergyPlanRepository>();
        _service = new EnergyService(_repository);

        // Load test data from JSON file
        var jsonPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", "MockPlans.json");
        var json = File.ReadAllText(jsonPath);
        _plans = JsonSerializer.Deserialize<List<EnergyPlan>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        });
    }
    
    [TestCase(1000, 
        "energyOne,planOne,108.68\nenergyThree,planThree,111.25\nenergyTwo,planTwo,120.23\nenergyFour,planFour,121.33")]
    [TestCase(2000, 
        "energyThree,planThree,205.75\nenergyOne,planOne,213.68\nenergyFour,planFour,215.83\nenergyTwo,planTwo,235.73")]
    public void Calculate_WithGivenUsage_ReturnsExpectedOutput(double usage, string expectedOutput)
    {
        // Arrange
        A.CallTo(() => _repository.GetPlans()).Returns(_plans);

        // Act
        string result = _service.Calculate(usage);

        // Normalize line endings
        result = result.Replace(Environment.NewLine, "\n");

        // Assert exact match
        Assert.That(expectedOutput, Is.EqualTo(result));
    }
}