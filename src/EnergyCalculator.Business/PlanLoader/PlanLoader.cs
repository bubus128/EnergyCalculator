using System.Text.Json;
using EnergyCalculator.Data.Models;

namespace EnergyCalculator.Business.PlanReader;

public class PlanLoader(JsonSerializerOptions options) : IPlanLoader
{
    public async Task<IEnumerable<EnergyPlan>> LoadAsync(Stream stream) =>
        await JsonSerializer.DeserializeAsync<List<EnergyPlan>>(stream, options)
        ?? Enumerable.Empty<EnergyPlan>();
}