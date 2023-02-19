
namespace FFAuctionBrokerLib.Models;

public class AuctionItem
{
	public uint Id { get; set; }

	public uint ItemId { get; set; }

	public bool Stack { get; set; }

	public uint Seller { get; set; }

	public string? SellerName { get; set; }

	public uint Date { get; set; }

	public uint Price { get; set; }

	public string? BuyerName { get; set; }

	public uint Sale { get; set; }

	public uint SellDate { get; set; }

}

