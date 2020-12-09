using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDb.Repositories.Models;
using MongoDb.Repositories.Repositories;

namespace BooksSite.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BooksApiController : ControllerBase
    {
        private readonly IBookRepository _repository;

        public BooksApiController(IBookRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> Get()
        {
            var books = await _repository.GetAllBooks();
            return new ObjectResult(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> Get(long id)
        {
            var book = await _repository.GetBook(id);

            if (book == null)
            {
                return new NotFoundResult();
            }
            
            return new ObjectResult(book);
        }
        
    }
}