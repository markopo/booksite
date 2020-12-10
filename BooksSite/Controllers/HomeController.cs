using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksSite.Mapper;
using BooksSite.Models;
using BooksSite.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDb.Repositories.Repositories;

namespace BooksSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookSearchService _bookSearchService;
        private readonly IBookRepository _repository;

        public HomeController(IBookSearchService bookSearchService, IBookRepository repository)
        {
            _bookSearchService = bookSearchService;
            _repository = repository;
        }
        
        [HttpGet]
        public IActionResult Index(string q)
        {
            var result = !string.IsNullOrEmpty(q) ? _bookSearchService.Retrieve(q) : BookResult.Create();

            if (result.Success && result.GoogleBooks.Any())
            {
               Task.Run(() => InsertToDb(result.GoogleBooks, q)).Wait();
            }
            
            return View("Index", result);
        }

        private async Task InsertToDb(List<GoogleBook> googleBooks, string q)
        {
            var books = googleBooks.Select(GoogleBooksMapper.Map).ToList();
            books.ForEach(b => b.Tag = q);
            await _repository.CreateManyIfNotExists(books.AsEnumerable());
        }
        
        
    }
}