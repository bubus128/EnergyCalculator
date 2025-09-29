// See https://aka.ms/new-console-template for more information

using EnergyCalculator.Business.EnergyService;
using EnergyCalculator.Console.CommandDispatcher;
using EnergyCalculator.Console.Exceptions;
using EnergyCalculator.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

const string exitCommandName = "exit";

var builder = Host.CreateApplicationBuilder(args);

// Register services
builder.Services.AddSingleton<IEnergyPlanRepository, EnergyPlanRepository>();
builder.Services.AddSingleton<IEnergyService, EnergyService>();
builder.Services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

using var host = builder.Build();

// Get dispatcher from container
var dispatcher = host.Services.GetRequiredService<ICommandDispatcher>();

PrintIntro();

while (true)
{
    try
    {
        // Read a command
        var input = Console.ReadLine();

        if (string.Equals(input, exitCommandName, StringComparison.OrdinalIgnoreCase))
            break;

        // Execute a command
        Console.WriteLine(await dispatcher.DispatchAsync(input ?? string.Empty));
    }
    catch (InvalidCommandException)
    {
        Console.WriteLine("Invalid command");
        PrintAvailableCommands();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

void PrintIntro()
{
    Console.WriteLine("=== Energy Calculator Console ===");
    PrintAvailableCommands();
    Console.WriteLine();
    Console.WriteLine("Type a command and press Enter:");
}

void PrintAvailableCommands()
{
    Console.WriteLine("Available commands:");
    Console.WriteLine("  a <path>     : process the file at the given path");
    Console.WriteLine("  b <number>   : run calculation with the given number");
    Console.WriteLine("  exit         : quit the application");
}
