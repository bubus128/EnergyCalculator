using EnergyCalculator.Business.EnergyService;
using EnergyCalculator.Console.Exceptions;

namespace EnergyCalculator.Console.CommandDispatcher;

public class CommandDispatcher : ICommandDispatcher
{
    private const string InputCommandName = "input";
    private const string CalculateCommandName = "annual_cost";
    
    private readonly IEnergyService _energyService;

    private readonly Dictionary<string, Func<string[], Task<string>>> _commands;

    public CommandDispatcher(IEnergyService energyService)
    {
        _energyService = energyService;
        _commands = new Dictionary<string, Func<string[], Task<string>>>
        {
            { InputCommandName, HandleInputAsync },
            { CalculateCommandName, HandleCalculateAsync }
        };
    }
    
    public async Task<string> DispatchAsync(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new InvalidCommandException(string.Empty);

        var parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        var commandName = parts[0].ToLowerInvariant();
        var args = parts.Length > 1
            ? parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
            : Array.Empty<string>();

        if (!_commands.TryGetValue(commandName, out var action))
        {
            throw new InvalidCommandException(commandName);
            
        }
        
        return await action(args);
    }
    
    private async Task<string> HandleInputAsync(string[] args)
    {
        if (args.Length != 1)
        {
            throw new ArgumentsNumberException(1, args.Length);
        }

        await _energyService.Input(args[0]);
        return "File processing complete";
    }

    private async Task<string> HandleCalculateAsync(string[] args)
    {
        if (args.Length != 1 || !float.TryParse(args[0], out var usage))
        {
            throw new ArgumentsNumberException(1, args.Length);
        }

        return _energyService.Calculate(usage);
    }
}