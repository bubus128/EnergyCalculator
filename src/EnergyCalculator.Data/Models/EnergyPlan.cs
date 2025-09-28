namespace EnergyCalculator.Data.Models;

public class EnergyPlan
{
    public string PlanName { get; set; }
    public string SupplierName  { get; set; }
    public Price[] Prices { get; set; }
}