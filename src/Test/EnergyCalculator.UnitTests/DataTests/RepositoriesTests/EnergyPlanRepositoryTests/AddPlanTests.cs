using EnergyCalculator.Data.Models;
using EnergyCalculator.Data.Repositories;

namespace EnergyCalculator.UnitTests.DataTests.RepositoriesTests.EnergyPlanRepositoryTests;

[TestFixture]
public class AddPlanTests
{
    private EnergyPlanRepository _repository;

    [SetUp]
    public void Setup()
    {
        _repository = new EnergyPlanRepository();
    }
    
    [Test]
    public void AddPlan_WhenCalled_ShouldAddPlanToRepository()
    {
        // Arrange
        var plan = new EnergyPlan
        {
            PlanName = "TestPlan",
            SupplierName = "TestSupplier",
            Prices = []
        };

        // Act
        _repository.AddPlan(plan);

        // Assert
        Assert.That(_repository.GetPlans(), Has.Count.EqualTo(1));
        Assert.That(_repository.GetPlans()[0], Is.EqualTo(plan));
    }

    [Test]
    public void AddPlan_MultiplePlans_ShouldPreserveOrder()
    {
        // Arrange
        var plan1 = new EnergyPlan
        {
            PlanName = "Plan1",
            SupplierName = "Supplier1",
            Prices = []
        };
        var plan2 = new EnergyPlan
        {
            PlanName = "Plan2",
            SupplierName = "Supplier2",
            Prices = []
        };

        // Act
        _repository.AddPlan(plan1);
        _repository.AddPlan(plan2);
        var result = _repository.GetPlans();

        // Assert
        Assert.That(result[0], Is.EqualTo(plan1));
        Assert.That(result[1], Is.EqualTo(plan2));
    }
}