using EnergyCalculator.Data.Models;

namespace EnergyCalculator.Business.PlanReader;

public interface IPlanLoader
{
    Task<IEnumerable<EnergyPlan>> LoadAsync(Stream stream);
}