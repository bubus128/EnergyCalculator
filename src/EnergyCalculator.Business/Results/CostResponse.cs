using System.Globalization;

namespace EnergyCalculator.Business.Results;

public class CostResponse
{
    public string SupplierName { get; set; }
    public string PlanName { get; set; }
    public double Cost { get; set; }
    private double RoundCost => Math.Round(Cost, 2, MidpointRounding.AwayFromZero);
    
    public override string ToString()
    {
        return string.Create(
            CultureInfo.InvariantCulture,
            $"{SupplierName},{PlanName},{RoundCost}"
        );
    }
}