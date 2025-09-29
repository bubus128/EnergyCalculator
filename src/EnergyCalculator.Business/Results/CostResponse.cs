using System.Globalization;

namespace EnergyCalculator.Business.Results;

public class CostResponse
{
    public required string SupplierName { get; init; }
    public required string PlanName { get; init; }
    public double Cost { get; init; }
    private double RoundCost => Math.Round(Cost, 2, MidpointRounding.AwayFromZero);
    
    public override string ToString()
    {
        return string.Create(
            CultureInfo.InvariantCulture,
            $"{SupplierName},{PlanName},{RoundCost}"
        );
    }
}