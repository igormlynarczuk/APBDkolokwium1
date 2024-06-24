using BlazorApp1.Models_and_DTOs;
using BlazorApp1.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using System.Threading.Tasks;

namespace BlazorApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository _booksRepository;

        public BooksController(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        [HttpGet("{id}/genres")]
        public async Task<IActionResult> GetGenresBook(int id)
        {
            if (!await _booksRepository.DoesBookExist(id))
                return NotFound($"Book with given ID - {id} doesn't exist");

            var bookGenres = await _booksRepository.GetGenresBook(id);

            return Ok(bookGenres);
        }

        [HttpGet("{id}/authors")]
        public async Task<IActionResult> GetAuthorsBook(int id)
        {
            if (!await _booksRepository.DoesBookExist(id))
                return NotFound($"Book with given ID - {id} doesn't exist");

            var bookAuthors = await _booksRepository.GetAuthorsBook(id);

            return Ok(bookAuthors);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(NewBookWithAuthors newBookWithAuthors)
        {
            if (!await _booksRepository.DoesOwnerExist(newBookWithAuthors.OwnerId))
                return NotFound($"Owner with given ID - {newBookWithAuthors.OwnerId} doesn't exist");

            foreach (var author in newBookWithAuthors.Authors)
            {
                if (!await _booksRepository.DoesAuthorExist(author.PK))
                    return NotFound($"Author with given ID - {author.PK} doesn't exist");
            }

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var id = await _booksRepository.AddBook(new NewBookDTO()
                {
                    title = newBookWithAuthors.title,
                    OwnerId = newBookWithAuthors.OwnerId
                });

                foreach (var author in newBookWithAuthors.Authors)
                {
                    await _booksRepository.AddAuthorBook(id, author);
                }

                scope.Complete();
            }

            return Created(Request.Path.Value ?? "api/books", newBookWithAuthors);
        }
    }
}
