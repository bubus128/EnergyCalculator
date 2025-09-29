using EnergyCalculator.Data.Models;

namespace EnergyCalculator.Data.Repositories;

public interface IEnergyPlanRepository
{
    void AddPlan(EnergyPlan energyPlan);

    IReadOnlyList<EnergyPlan> GetPlans();
}