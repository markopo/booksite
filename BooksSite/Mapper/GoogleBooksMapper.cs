using BooksSite.Models;
using MongoDb.Repositories.Models;

namespace BooksSite.Mapper
{
    public class GoogleBooksMapper
    {
        public static Book Map(GoogleBook googleBook)
        {
            return new Book
            {
                Title = googleBook.Title,
                SubTitle = googleBook.SubTitle,
                Authors = googleBook.Authors,
                Publisher = googleBook.Publisher,
                PublishedDate = googleBook.PublishedDate,
                Description = googleBook.Description,
                PageCount = googleBook.PageCount,
                PrintType = googleBook.PrintType,
                Categories = googleBook.Categories,
                AverageRating = googleBook.AverageRating,
                ThumbNail = googleBook.ThumbNail,
                Language = googleBook.Language,
                CanonicalVolumeLink = googleBook.CanonicalVolumeLink,
                WebReaderLink = googleBook.WebReaderLink,
                PdfLink = googleBook.PdfLink,
                Isbn10 = googleBook.ISBN10,
                Isbn13 = googleBook.ISBN13
            };
        }
    }
}