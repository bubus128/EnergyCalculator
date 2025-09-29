using EnergyCalculator.Data.Models;
using EnergyCalculator.Data.Repositories;

namespace EnergyCalculator.UnitTests.DataTests.RepositoriesTests.EnergyPlanRepositoryTests;

[TestFixture]
public class GetPlansTests
{
    private EnergyPlanRepository _repository;

    [SetUp]
    public void Setup()
    {
        _repository = new EnergyPlanRepository();
    }

    [Test]
    public void GetPlans_WhenNoPlansAdded_ShouldReturnEmptyList()
    {
        // Act
        var result = _repository.GetPlans();

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetPlans_ReturnsReadOnlyList()
    {
        // Arrange
        var plan = new EnergyPlan { PlanName = "ReadOnlyTest", SupplierName = "Supplier" };
        _repository.AddPlan(plan);

        // Act
        var plans = _repository.GetPlans();

        // Assert
        Assert.That(plans, Is.InstanceOf<IReadOnlyList<EnergyPlan>>());

        var asCollection = plans as ICollection<EnergyPlan>;
        Assert.That(asCollection?.IsReadOnly, Is.True);
    }
}