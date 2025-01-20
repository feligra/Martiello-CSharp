using Martiello.Domain.Entity;
using MongoDB.Driver;

namespace Martiello.Infrastructure.Data
{
    public class DbContext
    {
        private readonly IMongoDatabase _database;

        public DbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);   
        }

        public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>("Customers");
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");
        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");

    }
}
