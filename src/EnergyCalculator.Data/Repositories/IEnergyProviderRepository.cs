using EnergyCalculator.Data.Models;

namespace EnergyCalculator.Data.Repositories;

public interface IEnergyProviderRepository
{
    Task AddSourceFile(string path);
    
    Task<EnergyPlan> GetProvider();
}