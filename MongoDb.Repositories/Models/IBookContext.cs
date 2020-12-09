using MongoDB.Driver;

namespace MongoDb.Repositories.Models
{
    public interface IBookContext
    {
        IMongoCollection<Book> Books { get; }
    }
}