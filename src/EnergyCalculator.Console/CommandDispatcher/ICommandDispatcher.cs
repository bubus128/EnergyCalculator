namespace EnergyCalculator.Console.CommandDispatcher;

public interface ICommandDispatcher
{
    Task<string> DispatchAsync(string input);
}