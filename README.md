# FFAuctionBroker

This is a C# console app that connects to a FFXI private server database and updates or maintains the auction house.

The connection string for the database is set in appsettings.json and the ahitems.csv should be modified to change default prices or stock amounts.

GetRandomName gets a random name from a list of old rockage members (Bismarck) to be the buyer or seller. This is meant to be a tribute to those people and if not changed, these names would need to be added to the table of disallowed_names in the login db.
