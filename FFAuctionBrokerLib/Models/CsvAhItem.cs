
namespace FFAuctionBrokerLib.Models;

public class CsvAhItem
{
	public uint ItemId { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool Sell1 { get; set; }

    public bool Buy1 { get; set; }

    public uint Price1 { get; set; }

    public uint Stock1 { get; set; }

    public bool Sell12 { get; set; }

    public bool Buy12 { get; set; }

    public uint Price12 { get; set; }

    public uint Stock12 { get; set; }

}

