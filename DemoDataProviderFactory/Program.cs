using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using System.Data;

class Program
{
    static string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true).Build();
        var strConnection = config["ConnectionString:MyStoreDB"];
        return strConnection;
    }
    
    static void ViewProduct()
    {
        DbProviderFactory factory = SqlClientFactory.Instance;
        using DbConnection connection = factory.CreateConnection();
        if (connection == null)
        {
            Console.WriteLine($"Unable to create the connnection object.");
            return;
        }
        connection.ConnectionString = GetConnectionString();
        connection.Open();
        DbCommand command = factory.CreateCommand();
        if (command == null )
        {
            Console.WriteLine($"Unable to create the command object.");
            return;
        }
        command.Connection = connection;
        command.CommandText = "Select ProductID, ProductName From Products";
        using DbDataReader dataReader = command.ExecuteReader();
        Console.WriteLine("*****Product list*****");
        while ( dataReader.Read())
        {
            Console.WriteLine($"ProductID: {dataReader["ProductId"]}," + $"ProductName: {dataReader["ProductName"]}.");
        }
    }
    static void Main(string[] args)
    {
        ViewProduct();
        Console.ReadLine();
    }
}