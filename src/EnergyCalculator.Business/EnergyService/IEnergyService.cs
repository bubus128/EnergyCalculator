namespace EnergyCalculator.Business.EnergyService;

public interface IEnergyService
{
    Task Input(string filePath);

    string Calculate(double usage);
}