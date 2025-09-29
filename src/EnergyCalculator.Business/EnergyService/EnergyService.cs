using System.Text.Json;
using EnergyCalculator.Business.Results;
using EnergyCalculator.Data.Models;
using EnergyCalculator.Data.Repositories;

namespace EnergyCalculator.Business.EnergyService;

public class EnergyService(IEnergyPlanRepository energyPlanRepository) : IEnergyService
{
    private const double Vat = 0.05;
    private const double PenceToPound = 0.01;
    private const int DaysInYear = 365;
    public async Task Input(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new  FileNotFoundException($"Energy plan file not found: {filePath}");
        }
        if (Path.GetExtension(filePath).ToLower() != ".json")
        {
            throw new ArgumentException($"Invalid file type. Please provide a JSON file: {filePath}");
        }

        await using var stream = File.OpenRead(filePath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        foreach (var energyPlan in await JsonSerializer.DeserializeAsync<List<EnergyPlan>>(stream, options) 
                                   ?? throw new InvalidOperationException())
        {
            energyPlanRepository.AddPlan(energyPlan);
        }
    }

    public string Calculate(double usage)
    {
        var plans = energyPlanRepository.GetPlans();
        var costResponsesList = plans.Select(plan => 
            new CostResponse
            {
                PlanName = plan.PlanName, 
                SupplierName = plan.SupplierName, 
                Cost = CalculateCost(plan, usage),
            }).ToList();
        return string.Join(Environment.NewLine, costResponsesList.OrderBy(x => x.Cost));
    }

    private double CalculateCost(EnergyPlan plan, double usage)
    {
        double cost = 0;
        foreach (var price in plan.Prices)
        {
            var threshold = price.Threshold ?? double.MaxValue;
            cost += double.Min(threshold, usage) * price.Rate;
            usage -= threshold;
        }

        return (cost + (plan.StandingCharge ?? 0) * DaysInYear) * (1 + Vat) * PenceToPound;
    }
}