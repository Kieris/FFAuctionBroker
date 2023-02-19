using System.Globalization;
using CsvHelper;
using FFAuctionBrokerLib.Models;

namespace FFAuctionBrokerLib.DataAccess
{
	public static class CsvDataAccess
	{
        /// <summary>
        /// Read the CSV and obtain the auction items
        /// </summary>
        /// <returns>All items from the CSV</returns>
        public static List<CsvAhItem> ReadCsvFile()
        {
            List<CsvAhItem> items = new();
            using (var reader = new StreamReader("ahitems.csv"))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<CsvAhItem>();
                    items = records.ToList();
                }
            }
            return items;
        }
    }
}

