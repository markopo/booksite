using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDb.Repositories.Models;

namespace MongoDb.Repositories.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooks();

        Task<Book> GetBook(long id);

        Task Create(Book book);

        Task<bool> Update(Book book);

        Task<bool> Delete(long id);

        Task<long> GetNextId();
    }
}