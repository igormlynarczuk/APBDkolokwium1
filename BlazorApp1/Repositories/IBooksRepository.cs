﻿using BlazorApp1.Models_and_DTOs;

namespace BlazorApp1.Repositories;

public interface IBooksRepository
{
    Task<bool> DoesBookExist(int id);
    Task<bool> DoesOwnerExist(int id);
    Task<bool> DoesGenreExist(int id);

    // Version with implicit transaction
    Task AddNewBookWithProcedures(NewBookWithGenres newBookWithProcedures);
    
    // Version with transaction scope
    Task<int> AddAnimal(NewBookDTO animal);
    Task AddProcedureAnimal(int animalId, ProcedureWithDate procedure);
}