namespace EnergyCalculator.Data.Models;

public class EnergyPlan
{
    public required string PlanName { get; init; }
    public required string SupplierName  { get; init; }
    public required Price[] Prices { get; set; }
    public double? StandingCharge { get; set; }
}