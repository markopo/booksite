using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using BooksSite.Models;

namespace BooksSite.Services
{
    public class BooksSearchService : IBookSearchService
    {
        private readonly string baseUrl = "https://www.googleapis.com/books/v1/volumes?";


        private string FindRaw(string q, string author = null, string title = null, string filter = null, string orderBy = null)
        {
            string query = string.Empty;
            string inauthor = string.Empty; 
            string intitle = string.Empty;
            query = Regex.Replace(q, @"\s{1,3}", "+"); 
            

            if(!string.IsNullOrEmpty(author))
            {
                inauthor = $"+inauthor:{author}";  
            }

            if (!string.IsNullOrEmpty(title))
            {
                intitle = $"+intitle:{title}"; 
            }

            if (!string.IsNullOrEmpty(filter))
            {
                filter = $"&filter={filter}"; 
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                orderBy = $"&orderBy={orderBy}"; 
            }

            var search = $"q={query}{inauthor}{intitle}{filter}{orderBy}";
            var searchUrl = baseUrl + search;
            var wc = new WebClient();
            var data = wc.DownloadString(searchUrl);
            return data; 
        }

       public BookResult Retrieve(string q)
       {
           var findBookResult = new BookResult
           {
               ErrorMessage = string.Empty, 
               GoogleBooks = new List<GoogleBook>()
           };


           try {
                var data = FindRaw(q);
                dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(data);
                findBookResult.Success = jsonData.items != null; 

                foreach(var item in jsonData.items)
                {
                    var volumeInfo = item.volumeInfo;

                    if(volumeInfo == null) continue;
                    
                    var isbn10 = ParseIsbn10(volumeInfo);
                    var isbn13 = ParseIsbn13(volumeInfo);
                    var categories = ParseCategories(volumeInfo);
                    var authors = ParseAuthors(volumeInfo);

                    if (string.IsNullOrEmpty(isbn10) && string.IsNullOrEmpty(isbn13)) continue;
                    
                    var gb = Create(volumeInfo, authors, categories, isbn10, isbn13);
                    findBookResult.GoogleBooks.Add(gb);

                }

                findBookResult.Success = true;
                findBookResult.ErrorMessage = string.Empty; 
            }
            catch(Exception ex)
            {
                findBookResult.ErrorMessage = ex.Message;
                findBookResult.Success = false;
                findBookResult.GoogleBooks = new List<GoogleBook>(); 
            }
            return findBookResult; 
       }
       
       private string ParseIsbn10(dynamic volumeInfo)
       {
           var isbn10 = string.Empty;

           if (volumeInfo.industryIdentifiers != null)
           {
               foreach (var ii in volumeInfo.industryIdentifiers)
               {
                   if (ii.type == "ISBN_10")
                   {
                       isbn10 = ii.identifier;
                   }
               }
           }

           return isbn10;
       }
       
       private string ParseIsbn13(dynamic volumeInfo)
       {
           var isbn13 = string.Empty;

           if (volumeInfo.industryIdentifiers != null)
           {
               foreach (var ii in volumeInfo.industryIdentifiers)
               {
                   if (ii.type == "ISBN_13")
                   {
                       isbn13 = ii.identifier;
                   }
               }
           }

           return isbn13;
       }

       private List<string> ParseCategories(dynamic volumeInfo)
       {
           var categories = new List<string>();
           if (volumeInfo.categories != null)
           {
               foreach(var cat in volumeInfo.categories)
               {
                   categories.Add(cat.Value); 
               }
           }

           return categories;
       }

       private List<string> ParseAuthors(dynamic volumeInfo)
       {
           var authors = new List<string>();
           if (volumeInfo.authors != null)
           {
               foreach (var aut in volumeInfo.authors)
               {
                   authors.Add(aut.Value);
               }
           }

           return authors;
       }

       private GoogleBook Create(dynamic volumeInfo, List<string> authors, List<string> categories, string isbn10, string isbn13)
       {
           return new GoogleBook
           {
               Title = volumeInfo.title, 
               SubTitle = volumeInfo.subtitle != null ? volumeInfo.subtitle : "", 
               Authors =  authors.ToArray(),
               Publisher = volumeInfo.publisher != null ? volumeInfo.publisher : "",
               PublishedDate = volumeInfo.publishedDate != null ? volumeInfo.publishedDate : "", 
               Description = volumeInfo.description != null ? volumeInfo.description : "", 
               PageCount = volumeInfo.pageCount != null ? volumeInfo.pageCount : "", 
               PrintType = volumeInfo.printType != null ? volumeInfo.printType : "", 
               Categories = categories.ToArray(), 
               AverageRating = volumeInfo.averageRating != null ? volumeInfo.averageRating : "0.0", 
               ThumbNail = volumeInfo.imageLinks != null && volumeInfo.imageLinks.thumbnail != null ? volumeInfo.imageLinks.thumbnail : "",
               Language = volumeInfo.language != null ? volumeInfo.language : "", 
               CanonicalVolumeLink = volumeInfo.canonicalVolumeLink != null ? volumeInfo.canonicalVolumeLink : "", 
               WebReaderLink = volumeInfo.accessInfo != null && volumeInfo.accessInfo.webReaderLink != null ? volumeInfo.accessInfo.webReaderLink : "", 
               PdfLink = volumeInfo.accessInfo != null && volumeInfo.accessInfo.pdf != null && volumeInfo.accessInfo.pdf.downloadLink != null ? volumeInfo.accessInfo.pdf.downloadLink : "",
               ISBN10 = isbn10, 
               ISBN13 = isbn13 
           };
       }
    }
}