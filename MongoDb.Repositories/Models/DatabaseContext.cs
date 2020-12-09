using System.Threading.Tasks;
using System.Xml;
using MongoDB.Driver;
using MongoDb.Repositories.Config;

namespace MongoDb.Repositories.Models
{
    public class DatabaseContext : IBookContext
    {
        private readonly IMongoDatabase _db;

        public IMongoCollection<Book> Books => _db.GetCollection<Book>("Books");
        public DatabaseContext(MongoDbConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            _db = client.GetDatabase(config.Database);

            Task.Run(SetUpIsbn10BookIndex).Wait();
        }

        private async Task SetUpIsbn10BookIndex()
        {
            var options = new CreateIndexOptions
            {
                Unique = true
            };
            
            var field = new StringFieldDefinition<Book>("Isbn10");
            var indexDefinition = new IndexKeysDefinitionBuilder<Book>().Ascending(field);
            var indexModel = new CreateIndexModel<Book>(indexDefinition, options);
            await _db.GetCollection<Book>("Books").Indexes.CreateOneAsync(indexModel);
        }
    }
}