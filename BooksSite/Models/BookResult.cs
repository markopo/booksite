using System.Collections.Generic;

namespace BooksSite.Models
{
    public class BookResult
    {
        public List<GoogleBook> GoogleBooks { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }

        public static BookResult Create()
        {
            return new BookResult {
                Success = false,
                ErrorMessage = string.Empty,
                GoogleBooks = new List<GoogleBook>()
            };
        }
    }
}