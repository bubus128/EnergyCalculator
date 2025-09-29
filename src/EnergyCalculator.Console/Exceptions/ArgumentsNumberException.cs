namespace EnergyCalculator.Console.Exceptions;

public class ArgumentsNumberException(int expected, int actual) : Exception
{
    private readonly string _details = $"Invalid arguments number. This Command accepts {expected} argument, but {actual} were given.";

    public override string Message 
        => $"{nameof(ArgumentsNumberException)} error : {_details}";
}