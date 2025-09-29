using EnergyCalculator.Business.PlanReader;
using EnergyCalculator.Data.Models;
using EnergyCalculator.Data.Repositories;

namespace EnergyCalculator.Console.Commands;

public class InputCommand(IPlanLoader planLoader, IEnergyPlanRepository repo) : ICommand
{
    public string Name => "input";

    private readonly IPlanLoader _planLoader = planLoader ?? throw new ArgumentNullException(nameof(planLoader));
    private readonly IEnergyPlanRepository _repo = repo ?? throw new ArgumentNullException(nameof(repo));
    
    public async Task<string> ExecuteAsync(string[] args)
    {
        if (args == null || args.Length == 0)
            throw new ArgumentException("'input' requires a file path argument. Example: input \"plans.json\"");

        var path = args[0];
        if (!File.Exists(path))
            throw new FileNotFoundException($"File not found: {path}");
        
        await using var stream = File.OpenRead(path);
        var plans = await _planLoader.LoadAsync(stream) ?? Enumerable.Empty<EnergyPlan>();
        
        var added = 0;
        foreach (var p in plans)
        {
            _repo.AddPlan(p);
            added++;
        }

        return $"Imported {added} plan(s) from '{path}'.";
    }
}