using BlazorApp1.Models_and_DTOs;
using BlazorApp1.Repositories;

namespace BlazorApp1.Controllers;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class BooksController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IBooksRepository _booksRepository;
        public AnimalsController(IBooksRepository animalsRepository)
        {
            _booksRepository = animalsRepository;
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnimal(int id)
        {
            if (!await _booksRepository.DoesBookExist(id))
                return NotFound($"Animal with given ID - {id} doesn't exist");

            var animal = await _booksRepository.GetGenresBook(id);
            
            return Ok(animal);
        }
        

        // Version with implicit transaction
        [HttpPost]
        public async Task<IActionResult> AddBook(NewBookWithGenres newBookWithGenres)
        {
            if (!await _booksRepository.DoesOwnerExist(newBookWithGenres.OwnerId))
                return NotFound($"Owner with given ID - {newBookWithGenres.OwnerId} doesn't exist");

            foreach (var genre in newBookWithGenres.Genres)
            {
                if (!await _booksRepository.DoesGenreExist(genre.PK))
                    return NotFound($"Procedure with given ID - {genre.PK} doesn't exist");
            }

            await _booksRepository.AddNewBookWithProcedures(newBookWithGenres);

            return Created(Request.Path.Value ?? "api/animals", newBookWithGenres);
        }
        
        // Version with transaction scope
        [HttpPost]
        [Route("with-scope")]
        public async Task<IActionResult> AddBookV2(NewBookWithGenres newBookWithGenres)
        {

            if (!await _booksRepository.DoesOwnerExist(newBookWithGenres.OwnerId))
                return NotFound($"Owner with given ID - {newBookWithGenres.OwnerId} doesn't exist");

            foreach (var genre in newBookWithGenres.Genres)
            {
                if (!await _booksRepository.DoesGenreExist(genre.PK))
                    return NotFound($"Procedure with given ID - {genre.PK} doesn't exist");
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

            return Created(Request.Path.Value ?? "api/animals", newBookWithGenres);
        }
    }
}