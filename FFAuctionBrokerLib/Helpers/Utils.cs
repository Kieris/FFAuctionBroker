
using FFAuctionBrokerLib.DataAccess;
using FFAuctionBrokerLib.Models;

namespace FFAuctionBrokerLib.Helpers;

public class Utils
{
    private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static Random rand = new Random();

    /// <summary>
    /// Converts a DateTime to the timestamp value needed for the database
    /// </summary>
    public static uint ConvertToTimestamp(DateTime value)
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

                var auctionItem = new AuctionItem(item);
                for (int i = 0; i < stock; i++)
                {
                    sql.CreateAhItem(auctionItem);
                }

                //create a sold starter item to show price
                UpdateSoldItem(auctionItem);
                sql.CreateAhItem(auctionItem);
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

                //need to account for stacks and singles
                var dbItems = sql.GetAhItemsById(item.ItemId, item.Sell12);

                 
                if(dbItems is not null && dbItems.Count < stock)
                {
                    var needed = stock - dbItems.Count;
                    //the amount of each item to add to the database             
                    for (int i = 0; i < needed; i++)
                    {
                        sql.CreateAhItem(new AuctionItem(item));
                    }
                }

                var playerItems = sql.GetPlayerUnsoldAhItems(item.ItemId);
                if (playerItems?.Count > 0)
                {
                    foreach(var pitem in playerItems)
                    {
                        //buy players item if price is not higher than defaults
                        if(pitem.Stack)
                        {
                            if(pitem.Price <= item.Price12)
                            {
                                BuyItem(sql, pitem);
                            }
                        }
                        else
                        {
                            if(pitem.Price <= item.Price1)
                            {
                                BuyItem(sql, pitem);
                            }
                        }
                    }
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
    private static void BuyItem(MariaDbCrud sql, AuctionItem item)
    {
        UpdateSoldItem(item);

        sql.UpdateAhItem(item);

        var dboxItem = new DeliveryItem()
        {
            ItemId = 65535,//gil id
            Quantity = item.Sale,
            Sender = "AH-Jeuno",
            CharId = item.Seller,
            CharName = item.SellerName,
            Box = 1
        };

        sql.GetMaxSlot(dboxItem);

        sql.CreateDeliveryItem(dboxItem);
    }

    private static void UpdateSoldItem(AuctionItem item)
    {
        item.BuyerName = GetRandomName();
        item.SellDate = ConvertToTimestamp(DateTime.Now);
        item.Sale = item.Price;
    }

    /// <summary>
    /// Get a random name from a list of old rockage members (Bismarck) to be the buyer or seller.
    /// This is meant to be tribute to those people and these names would need to be added to the
    /// table of disallowed_names in the login db.
    /// </summary>
    /// <returns>A random name</returns>
    public static string GetRandomName()
    {
        string[] sellers = {"Vizzini" , "Spudz", "Durandel", "Bellemithra", "Vaulout", "Samsonk",
        "Kulak", "Pepsimaus", "Noobles", "Brunokurt", "Shatow", "Khiinroye", "Quikstrike", "Thadin",
        "Pinrakjud", "Foxfall", "Slaxx", "Flintyoungblood"};

        var index = rand.Next(0, sellers.Length - 1);

        return sellers[index];
    }
}

