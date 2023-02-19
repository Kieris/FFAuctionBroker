
namespace FFAuctionBrokerLib.Models;

public class AuctionItem
{
	private static Random rand = new Random();

	public uint Id { get; set; }

	public uint ItemId { get; set; }

	public bool Stack { get; set; }

	public uint Seller { get; set; }

	public string SellerName { get; set; } = string.Empty;

	public uint Date { get; set; }

	public uint Price { get; set; }

	public string BuyerName { get; set; } = string.Empty;

	public uint Sale { get; set; }

	public uint SellDate { get; set; }

	/// <summary>
	/// Get a random name from a list of old rockage members (Bismarck) to be the buyer or seller.
	/// This is meant to be tribute to those people and these names would need to be added to the
	/// table of disallowed_names in the login db.
	/// </summary>
	/// <returns></returns>
	public static string GetRandomSeller()
	{		
		string[] sellers = {"Vizzini" , "Spudz", "Durandel", "Bellemithra", "Vaulout", "Samsonk",
		"Kulak", "Pepsimaus", "Noobles", "Brunokurt", "Shatow", "Khiinroye", "Quikstrike", "Thadin",
        "Pinrakjud", "Foxfall", "Slaxx", "Flintyoungblood"};

		var index = rand.Next(0, sellers.Length - 1);

		return sellers[index];
	}
}

