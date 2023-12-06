using System.Collections.ObjectModel;
using MongoDB.Bson;
using MongoDB.Driver;
using RiaApi.Models;

namespace RiaApi.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private IMongoCollection<Customer> _collection;

        public CustomerRepository(IConfiguration configuration)
        {

            var connectionString = configuration.GetSection("ConnectionString").GetValue<string>("MONGODB_URI");
            Console.WriteLine(connectionString);
            if (connectionString == null)
            {
                Console.WriteLine("You must set your 'MONGODB_URI' environment variable. To learn how to set it, see https://www.mongodb.com/docs/drivers/csharp/current/quick-start/#set-your-connection-string");
                Environment.Exit(0);
            }

            var client = new MongoClient(connectionString);
            Console.WriteLine("connected/get_collect");
            IMongoCollection<Customer> collection = client.GetDatabase("db").GetCollection<Customer>("customers");
            _collection = collection;
        }

        public async Task<List<Customer>?> GetAllAsync()
        {
            var documents = await _collection.Find(new BsonDocument()).ToListAsync();
            return documents;
        }

        public async Task CreateManyAsync(IEnumerable<Customer> customers)
        {
            await _collection.InsertManyAsync(customers);
        }

        public async Task<bool> HasExistentIdAsync(IEnumerable<Customer> customers)
        {
            var filter = Builders<Customer>.Filter.In(x => x.Id, customers.Select(x => x.Id));
            var result = await _collection.Find(filter).ToListAsync();

            return result.Count > 0;
        }

        public List<Customer>? GetInternalArray()
        {
            var sortDefinition = Builders<Customer>.Sort.Ascending(x => x.LastName).Ascending(x => x.FirstName);
            var result = _collection.Find(new BsonDocument()).Sort(sortDefinition).ToList();
            return result;
        }
    }
}