using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDb.Repositories.Models;

namespace MongoDb.Repositories.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IBookContext _context;

        public BookRepository(IBookContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _context.Books.Find(_ => true).ToListAsync();
        }

        public async Task<Book> GetBook(long id)
        {
            var filter = Builders<Book>.Filter.Eq(m => m.Id, id);
            return await _context.Books.Find(filter).FirstOrDefaultAsync();
        }

        public async Task Create(Book book)
        {
            await _context.Books.InsertOneAsync(book);
        }

        public async Task<bool> Update(Book book)
        {
            var ok = await _context.Books.ReplaceOneAsync(b => b.Id == book.Id, book);
            return ok.IsAcknowledged && ok.MatchedCount > 0;
        }

        public async Task<bool> Delete(long id)
        {
            var filter = Builders<Book>.Filter.Eq(b => b.Id, id);
            var deleted = await _context.Books.DeleteOneAsync(filter);
            return deleted.IsAcknowledged && deleted.DeletedCount > 0;
        }

        public async Task<long> GetNextId()
        {
            return await _context.Books.CountDocumentsAsync(new BsonDocument()) + 1;
        }
    }
}