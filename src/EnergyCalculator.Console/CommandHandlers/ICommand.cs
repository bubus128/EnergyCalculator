namespace EnergyCalculator.Console.CommandHandlers;

public interface ICommand
{
    Task ExecuteAsync(string[] args);
}