using FFAuctionBrokerLib.DataAccess;
using FFAuctionBrokerLib.Helpers;
using FFAuctionBrokerLib.Models;
using Microsoft.Extensions.Configuration;

StartMainLoop();

Console.ReadLine();

void StartMainLoop()
{
    Console.WriteLine("What service would you like to start?");
    Console.WriteLine("1 to start broker (maintain item quantity and purchase user items");
    Console.WriteLine("2 to initialize Auction House with all items from csv");
    Console.WriteLine("3 to maintain item quantities only");

    string? input = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(input))
    {
        MariaDbCrud sql = new MariaDbCrud(GetConnectionString()!);

        //Don't necessarily need to parse the input, but... meh
        if (input is not null && int.TryParse(input, out int val))
        {
            var csvItems = CsvDataAccess.ReadCsvFile();
            string result;
            switch (val)
            {
                case 1:
                    StartMaintenanceCheck(sql, csvItems);
                    break;
                case 2:
                    Console.WriteLine("Adding all items to database...");
                    result = Utils.Initialize(sql, csvItems);
                    Console.WriteLine(result);
                    break;
                case 3:
                    Utils.BuyPlayerItems = false;
                    StartMaintenanceCheck(sql, csvItems);
                    break;
                default:
                    Console.WriteLine("The input you entered is not a valid option.");
                    break;
            }
        }
    }
    else
    {
        Console.WriteLine("You must provide an input.");
        StartMainLoop();
    }
}

static string? GetConnectionString(string connectionStringName = "Default")
{
    var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
    var builder = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{environment}.json", optional: true)
        .AddUserSecrets<Program>()
        .AddEnvironmentVariables();

    var config = builder.Build();

    return config.GetConnectionString(connectionStringName)!;
}

void StartMaintenanceCheck(MariaDbCrud sql, List<CsvAhItem>? csvItems)
{
    System.Timers.Timer timer = new(interval: 1200000);//20 minutes in millisecs
    timer.Elapsed += (sender, e) => MaintenanceCheck(sql, csvItems);
    timer.Start();
}

void MaintenanceCheck(MariaDbCrud sql, List<CsvAhItem>? csvItems)
{
    Console.WriteLine("Starting a maintenance check...");
    var result = Utils.Maintain(sql, csvItems!);
    Console.WriteLine(result);
} 


