

using FFAuctionBrokerLib.Models;

namespace FFAuctionBrokerLib.DataAccess;

public class MariaDbCrud
{
    private readonly string _connectionString;
    private MariaDbDataAccess db = new MariaDbDataAccess();

    /// <summary>
    /// Constructor for MariaDbCrud
    /// </summary>
    /// <param name="connectionString">DB connection string</param>
    public MariaDbCrud(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Gets all items from Auction House table. Risky and should probably never be used.
    /// </summary>
    /// <returns>All auction items</returns>
    public List<AuctionItem> GetAllAhItems()
    {
        string sql = "select * from auction_house";

        return db.LoadData<AuctionItem, dynamic>(sql, new { }, _connectionString);
    }

    /// <summary>
    /// Gets an item from Auction House using its id
    /// </summary>
    /// <param name="id">The id of the item</param>
    /// <returns>The item if exists</returns>
    public AuctionItem GetAhItemById(int id)
    {
        string sql = "select * from auction_house where id = @Id";
        AuctionItem? output = new AuctionItem();

        output = db.LoadData<AuctionItem, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

        if (output == null)
        {
            // do something to tell the user that the record was not found
            return null;
        }
        return output;
    }

    /// <summary>
    /// Creates an item for the Auction House
    /// </summary>
    /// <param name="item">The item to be added to the Auction House table</param>
    public void CreateAhItem(AuctionItem item)
    {
        // Save the basic contact
        string sql = "insert into auction_house (itemid, stack, seller, seller_name, date, " +
            "price, buyer_name, sale, sell_date) values (@ItemId, @Stack, @Seller, @SellerName, " +
            "@Date, @Price, @BuyerName, @Sale, @SellDate);";

        db.SaveData(sql, item, _connectionString);
    }

    /// <summary>
    /// Updates an item in the Auction House (generally if item sold)
    /// </summary>
    /// <param name="item">The item to update</param>
    public void UpdateAhItem(AuctionItem item)
    {
        string sql = "update auction_house set buyer_name = @BuyerName, sale = @Sale, sell_date = @SellDate" +
            " where Id = @Id";
        db.SaveData(sql, item, _connectionString);
    }

    /// <summary>
    /// Remove an item from the Auction House. This will rarely be used if ever.
    /// </summary>
    /// <param name="id">The id of the item to remove</param>
    public void RemoveAhItem(int id)
    {
        string sql = "delete from auction_house where id = @Id";
        db.SaveData(sql, new { Id = id }, _connectionString);
    }
}


