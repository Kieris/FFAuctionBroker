
using FFAuctionBrokerLib.DataAccess;
using FFAuctionBrokerLib.Models;

namespace FFAuctionBrokerLib.Helpers;

public class Utils
{
    private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public static bool LogSells = true;
    private static Random rand = new Random();

    /// <summary>
    /// Converts a DateTime to the timestamp value needed for the database
    /// </summary>
    private static uint ConvertToTimestamp(DateTime value)
    {
        TimeSpan elapsedTime = value - Epoch;
        return (uint)elapsedTime.TotalSeconds;
    }

    /// <summary>
    /// Adds the amount of stock for every item in the CSV to the database
    /// </summary>
    /// <returns>Complete or an error message</returns>
    public static string Initialize(MariaDbCrud sql, List<CsvAhItem> ahItems)
    {
        try
        {
            foreach (var item in ahItems)
            {
                //the amount of each item to add to the database
                var stock = item.Sell12 ? item.Stock1 : item.Stock12;

                for (int i = 0; i < stock; i++)
                {
                    sql.CreateAhItem(new AuctionItem()
                    {
                        ItemId = item.ItemId,
                        Stack = item.Sell12,
                        Price = item.Sell12 ? item.Price12 : item.Price1,
                        Date = ConvertToTimestamp(DateTime.Now)
                    });
                }
            }
        }
        catch(Exception e)
        {
            return e.Message;
        }
        return "Complete";
    }

    /// <summary>
    /// Searchs for items that do not have the appropriate stock and updates the stock if needed. It also
    /// purchases items for sale by player that do not have a price gt the default.
    /// </summary>
    /// <returns>Complete or an error message</returns>
    public static string Maintain(MariaDbCrud sql, List<CsvAhItem> ahItems)
    {
        try
        {
            foreach (var item in ahItems)
            {
                var stock = item.Sell12 ? item.Stock1 : item.Stock12;

                var dbItems = sql.GetAhItemsById(item.ItemId);

                //need to account for stacks and singles HERE 
                if(dbItems is not null && dbItems.Count < stock)
                {
                    var needed = stock - dbItems.Count;
                    //the amount of each item to add to the database             
                    for (int i = 0; i < needed; i++)
                    {
                        sql.CreateAhItem(new AuctionItem()
                        {
                            ItemId = item.ItemId,
                            Stack = item.Sell12,
                            Price = item.Sell12 ? item.Price12 : item.Price1,
                            Date = ConvertToTimestamp(DateTime.Now)
                        });
                    }
                }

                var playerItems = sql.GetPlayerUnsoldAhItems(item.ItemId);
                if (playerItems?.Count > 0 && LogSells)
                {
                    foreach(var pitem in playerItems)
                    {
                        //buy players item if price is not higher than defaults
                        if(pitem.Stack)
                        {
                            if(pitem.Price <= item.Price12)
                            {

                            }
                        }
                        else
                        {
                            if(pitem.Price <= item.Price1)
                            {

                            }
                        }
                    }
                    Console.WriteLine(playerItems?.FirstOrDefault()?.ItemId);
                }
            }         
        }
        catch (Exception e)
        {
            return e.Message;
        }
        return "Complete";
    }

    /// <summary>
    /// Buys an item that meets the requirements and updates the database
    /// </summary>
    private static void BuyItem(MariaDbCrud sql, AuctionItem item, uint sellerPrice)
    {
        var buyer = GetRandomSeller();
        item.BuyerName = buyer;
        item.SellDate = ConvertToTimestamp(DateTime.Now);
        item.Sale = sellerPrice;

        sql.UpdateAhItem(item);

        sql.CreateDeliveryItem(new DeliveryItem()
        {
            ItemId = item.Id,//this should probably be gil id
            Quantity = item.Sale,
            //SenderId
        });

    }

    /// <summary>
    /// Get a random name from a list of old rockage members (Bismarck) to be the buyer or seller.
    /// This is meant to be tribute to those people and these names would need to be added to the
    /// table of disallowed_names in the login db.
    /// </summary>
    /// <returns>A random name</returns>
    public static string GetRandomSeller()
    {
        string[] sellers = {"Vizzini" , "Spudz", "Durandel", "Bellemithra", "Vaulout", "Samsonk",
        "Kulak", "Pepsimaus", "Noobles", "Brunokurt", "Shatow", "Khiinroye", "Quikstrike", "Thadin",
        "Pinrakjud", "Foxfall", "Slaxx", "Flintyoungblood"};

        var index = rand.Next(0, sellers.Length - 1);

        return sellers[index];
    }
}

