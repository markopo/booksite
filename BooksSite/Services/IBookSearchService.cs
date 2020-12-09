using BooksSite.Models;

namespace BooksSite.Services
{
    public interface IBookSearchService
    {
        BookResult Retrieve(string q);
    }
}