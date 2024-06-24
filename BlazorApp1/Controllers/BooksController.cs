using BlazorApp1.Models_and_DTOs;
using BlazorApp1.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

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

        [HttpPost]
        public async Task<IActionResult> AddBook(NewBookWithGenres newBookWithGenres)
        {
            if (!await _booksRepository.DoesOwnerExist(newBookWithGenres.OwnerId))
                return NotFound($"Owner with given ID - {newBookWithGenres.OwnerId} doesn't exist");

            foreach (var genre in newBookWithGenres.Genres)
            {
                if (!await _booksRepository.DoesGenreExist(genre.PK))
                    return NotFound($"Genre with given ID - {genre.PK} doesn't exist");
            }

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var id = await _booksRepository.AddBook(new NewBookDTO()
                {
                    PK = newBookWithGenres.PK,
                    title = newBookWithGenres.title,
                    OwnerId = newBookWithGenres.OwnerId
                });

                foreach (var genre in newBookWithGenres.Genres)
                {
                    await _booksRepository.AddGenreBook(id, genre);
                }

                scope.Complete();
            }

            return Created(Request.Path.Value ?? "api/books", newBookWithGenres);
        }
    }
}
