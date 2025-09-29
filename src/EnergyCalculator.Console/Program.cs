using System.Text.Json;
using EnergyCalculator.Business.EnergyService;
using EnergyCalculator.Business.PlanReader;
using EnergyCalculator.Console.AppRunner;
using EnergyCalculator.Console.CommandDispatcher;
using EnergyCalculator.Console.Commands;
using EnergyCalculator.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Register json serialization options
builder.Services.AddSingleton(new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
});

// Register services
builder.Services.AddSingleton<IEnergyPlanRepository, EnergyPlanRepository>();
builder.Services.AddSingleton<IEnergyService, EnergyService>();
builder.Services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddSingleton<IPlanLoader, PlanLoader>();

// Register commands
builder.Services.AddSingleton<ICommand, InputCommand>();
builder.Services.AddSingleton<ICommand, CalculateCommand>();

// Register runner
builder.Services.AddSingleton<IAppRunner, AppRunner>();

using var host = builder.Build();

// Get dispatcher from container
var runner = host.Services.GetRequiredService<IAppRunner>();
await runner.RunAsync();
