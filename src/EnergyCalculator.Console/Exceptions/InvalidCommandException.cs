namespace EnergyCalculator.Console.Exceptions;

public class InvalidCommandException(string commandName) : Exception
{
    private readonly string _details = $"Invalid command name. Command {commandName} not found.";

    public override string Message 
        => $"{nameof(InvalidCommandException)} error : {_details}";
}