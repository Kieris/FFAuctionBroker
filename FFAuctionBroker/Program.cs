using FFAuctionBrokerLib.DataAccess;
using FFAuctionBrokerLib.Models;
using Microsoft.Extensions.Configuration;

MariaDbCrud sql = new MariaDbCrud(GetConnectionString()!);

Console.WriteLine("What service would you like to start?");
Console.WriteLine("1 to start broker (maintain item quantity and purchase user items");
Console.WriteLine("2 to initialize Auction House with all items from csv");
Console.WriteLine("3 to maintain item quantities only");

string input = Console.ReadLine();

//Don't necessarily need to parse the input, but... meh
if(input is not null && int.TryParse(input, out int val))
{
    var csvItems = CsvDataAccess.ReadCsvFile();

    var item = new AuctionItem()
    {
        ItemId = csvItems.First().ItemId,
        Price = csvItems.First().Price1,
       
    };

    var countSell1 = csvItems.Where(x => x.Sell1);
    var countSell12 = csvItems.Where(x => x.Sell12);

    Console.WriteLine(countSell1?.Count());
    Console.WriteLine(countSell12.Count());    


    switch (val)
    {
        case 1: 
            break;
        case 2: //do something
            break;
        case 3: //do something
            break;
        default:
            Console.WriteLine("The input you entered is not a valid option.");
            break;
    }
}

static string? GetConnectionString(string connectionStringName = "Default")
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json");

    var config = builder.Build();

    return config.GetConnectionString(connectionStringName)!;

}
