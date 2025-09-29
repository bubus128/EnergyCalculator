using EnergyCalculator.Business.EnergyService;

namespace EnergyCalculator.Console.Commands;

public class CalculateCommand(IEnergyService energyService) : ICommand
{
    public string Name => "annual_cost";
    
    private readonly IEnergyService _energyService = energyService ?? throw new ArgumentNullException(nameof(energyService));
    
    public Task<string> ExecuteAsync(string[] args)
    {
        if (args == null || args.Length == 0)
            throw new ArgumentException("'annual_cost' requires a numeric usage argument. Example: annual_cost 3500.5");
        
        if (!double.TryParse(args[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var usage))
            throw new ArgumentException("Could not parse this value. Use a number, e.g. 3500 or 3500.5");
        
        var result = _energyService.Calculate(usage);
        return Task.FromResult(result ?? string.Empty);
    }
}