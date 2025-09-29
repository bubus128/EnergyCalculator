using EnergyCalculator.Business.EnergyService;
using EnergyCalculator.Console.Commands;
using EnergyCalculator.Console.Exceptions;

namespace EnergyCalculator.Console.CommandDispatcher;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly Dictionary<string, ICommand> _commands;

    public CommandDispatcher(IEnumerable<ICommand> commands)
    {
        _commands = new Dictionary<string, ICommand>(StringComparer.OrdinalIgnoreCase);

        foreach (var command in commands)
        {
            if (!_commands.TryAdd(command.Name, command))
            {
                throw new InvalidOperationException(
                    $"Duplicate command name detected: {command.Name}");
            }
        }
    }

    /// <summary>
    /// Dispatches an input line to the appropriate command.
    /// </summary>
    /// <param name="inputLine">The raw input string from the user.</param>
    /// <returns>
    /// The result string from the command (or null if exit/terminate).
    /// </returns>
    public async Task<string?> DispatchAsync(string inputLine)
    {
        if (string.IsNullOrWhiteSpace(inputLine))
            return null;

        // First word is the command name
        var parts = inputLine.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        var commandName = parts[0];
        var rawArgs = parts.Length > 1 ? parts[1] : string.Empty;

        if (_commands.TryGetValue(commandName, out var command))
        {
            return await command.ExecuteAsync([rawArgs]);
        }

        throw new InvalidOperationException($"Unknown command: {commandName}");
    }
}