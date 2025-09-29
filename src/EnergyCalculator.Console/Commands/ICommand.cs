namespace EnergyCalculator.Console.Commands;

public interface ICommand
{
    string Name { get; }
    Task<string> ExecuteAsync(string[] args);
}