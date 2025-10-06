using Microsoft.Extensions.Configuration;
using MySqlConnector;  
using System.Data;

namespace EjemploCoreWeb.Repository;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _cs;

    public DbConnectionFactory(IConfiguration config)
    {
        _cs = config.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not found.");
    }

    public IDbConnection CreateConnection()
    {
        var cn = new MySqlConnection(_cs);
        cn.Open();
        return cn;
    }
}
