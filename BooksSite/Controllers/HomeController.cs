using System;
using System.Collections.Generic;
using BooksSite.Models;
using BooksSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace BooksSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookSearchService _bookSearchService;

        public HomeController(IBookSearchService bookSearchService)
        {
            _bookSearchService = bookSearchService;
        }
        
        [HttpGet]
        public IActionResult Index(string q)
        {
            BookResult result = !string.IsNullOrEmpty(q) ? _bookSearchService.Retrieve(q) : BookResult.Create();
            return View("Index", result);
        }
        
        
        
        
    }
}