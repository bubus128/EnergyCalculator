using EnergyCalculator.Console.CommandDispatcher;
using EnergyCalculator.Console.Exceptions;

namespace EnergyCalculator.Console.AppRunner;

public class AppRunner(ICommandDispatcher dispatcher) : IAppRunner
{
    private const string ExitCommandName = "exit";
    public async Task RunAsync()
    {
        PrintIntro();
        while (true)
        {
            try
            {
                // Read a command
                var input = System.Console.ReadLine();

                if (string.Equals(input, ExitCommandName, StringComparison.OrdinalIgnoreCase))
                    break;

                // Execute a command
                System.Console.WriteLine(await dispatcher.DispatchAsync(input ?? string.Empty));
            }
            catch (InvalidCommandException)
            {
                System.Console.WriteLine("Invalid command");
                PrintAvailableCommands();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }

    private void PrintIntro()
    {
        System.Console.WriteLine("=== Energy Calculator Console ===");
        PrintAvailableCommands();
        System.Console.WriteLine();
        System.Console.WriteLine("Type a command and press Enter:");
    }

    private void PrintAvailableCommands()
    {
        System.Console.WriteLine("Available commands:");
        System.Console.WriteLine("  input <path>           : process the file at the given path");
        System.Console.WriteLine("  annual_cost <number>   : run calculation with the given number");
        System.Console.WriteLine("  exit                   : quit the application");
    }
}