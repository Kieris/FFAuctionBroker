using System.Globalization;
using CsvHelper;
using FFAuctionBrokerLib.Models;

namespace FFAuctionBrokerLib.DataAccess
{
	public class CsvDataAccess
	{
        public List<AuctionItem> ReadCsvFile()
        {
            List<AuctionItem> items = new();
            using (var reader = new StreamReader("ahitems.csv"))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<AuctionItem>();
                    items = records.ToList();
                }
            }
            return items;
        }
    }
}

