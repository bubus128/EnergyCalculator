using EnergyCalculator.Data.Models;

namespace EnergyCalculator.Data.Repositories;

public class EnergyPlanRepository : IEnergyPlanRepository
{
    private readonly List<EnergyPlan> _plans = [];

    public void AddPlan(EnergyPlan energyPlan)
    {
        _plans.Add(energyPlan);
    }

    public IReadOnlyList<EnergyPlan> GetPlans()
    {
        return _plans.AsReadOnly();
    }
}