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

        // Version with implicit transaction
        [HttpPost]
        public async Task<IActionResult> AddAnimal(NewBookWithGenres NewBookWithGenres)
        {
            if (!await _booksRepository.DoesOwnerExist(Models_and_DTOs.NewBookWithGenres.OwnerId))
                return NotFound($"Owner with given ID - {newAnimalWithProcedures.OwnerId} doesn't exist");

            foreach (var procedure in newAnimalWithProcedures.Procedures)
            {
                if (!await _animalsRepository.DoesProcedureExist(procedure.ProcedureId))
                    return NotFound($"Procedure with given ID - {procedure.ProcedureId} doesn't exist");
            }

            await _animalsRepository.AddNewAnimalWithProcedures(newAnimalWithProcedures);

            return Created(Request.Path.Value ?? "api/animals", newAnimalWithProcedures);
        }
        
        // Version with transaction scope
        [HttpPost]
        [Route("with-scope")]
        public async Task<IActionResult> AddAnimalV2(NewAnimalWithProcedures newAnimalWithProcedures)
        {

            if (!await _animalsRepository.DoesOwnerExist(newAnimalWithProcedures.OwnerId))
                return NotFound($"Owner with given ID - {newAnimalWithProcedures.OwnerId} doesn't exist");

            foreach (var procedure in newAnimalWithProcedures.Procedures)
            {
                if (!await _animalsRepository.DoesProcedureExist(procedure.ProcedureId))
                    return NotFound($"Procedure with given ID - {procedure.ProcedureId} doesn't exist");
            }

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var id = await _animalsRepository.AddAnimal(new NewAnimalDTO()
                {
                    Name = newAnimalWithProcedures.Name,
                    Type = newAnimalWithProcedures.Type,
                    AdmissionDate = newAnimalWithProcedures.AdmissionDate,
                    OwnerId = newAnimalWithProcedures.OwnerId
                });

                foreach (var procedure in newAnimalWithProcedures.Procedures)
                {
                    await _animalsRepository.AddProcedureAnimal(id, procedure);
                }

                scope.Complete();
            }

            return Created(Request.Path.Value ?? "api/animals", newAnimalWithProcedures);
        }
    }
}