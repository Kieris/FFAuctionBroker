using MySqlConnector;
using System.Data;
using Dapper;

namespace FFAuctionBrokerLib.DataAccess;

public class MariaDbDataAccess
{
    /// <summary>
    /// Load data from the MariaDB database using a Dapper query
    /// </summary>
    /// <typeparam name="T">The type for the return value</typeparam>
    /// <typeparam name="U">The type for parameters (usually dynamic)</typeparam>
    /// <param name="sqlStatement">The SQL statement</param>
    /// <param name="parameters">An object representing parameters for the query</param>
    /// <param name="connectionString">The DB connection string</param>
    /// <returns>The value(s) obtained from the query</returns>
    public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionString)
    {
        using IDbConnection connection = new MySqlConnection(connectionString);

        List<T> rows = connection.Query<T>(sqlStatement, parameters).ToList();
        return rows;
    }

    /// <summary>
    /// Saves data to the MariaDB database
    /// </summary>
    /// <typeparam name="T">The type for parameters (usually dynamic)</typeparam>
    /// <param name="sqlStatement">The SQL statement</param>
    /// <param name="parameters">An object representing parameters for the statement</param>
    /// <param name="connectionString">The DB connection string</param>
    public void SaveData<T>(string sqlStatement, T parameters, string connectionString)
    {
        using IDbConnection connection = new MySqlConnection(connectionString);

        connection.Execute(sqlStatement, parameters);
    }
}

