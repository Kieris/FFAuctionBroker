
namespace FFAuctionBrokerLib.Models;

public class DeliveryItem
{
	public uint CharId { get; set; }

    public string? CharName { get; set; }

	public bool Box { get; set; }

	public ushort Slot { get; set; }

	public uint ItemId { get; set; }

	public uint ItemSubId { get; set; }

	public uint Quantity { get; set; }

	public uint SenderId { get; set; }

	public string? Sender { get; set; }

	public bool Received { get; set; }

	public bool Sent { get; set; }
}

