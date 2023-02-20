
namespace FFAuctionBrokerLib.Models;

/// <summary>
/// Class to represent an item in the delivery_box table.
/// Default values here reflect defaults for database even though several are the same as C# defaults.
/// Parameters that do not have defaults shown here need to be set before adding to db.
/// </summary>
public class DeliveryItem
{
	public uint CharId { get; set; }

    public string? CharName { get; set; }

	public sbyte Box { get; set; }

	public short Slot { get; set; } = 0;

	public uint ItemId { get; set; }

	public short ItemSubId { get; set; } = 0;

	public uint Quantity { get; set; }

	public uint SenderId { get; set; } = 0;

	public string? Sender { get; set; }

	public sbyte Received { get; set; } = 0;

	public sbyte Sent { get; set; } = 0;
}

