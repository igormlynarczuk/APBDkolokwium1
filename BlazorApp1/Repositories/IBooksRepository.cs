using BlazorApp1.Models_and_DTOs;

namespace BlazorApp1.Repositories;

public interface IBooksRepository
{
    Task<bool> DoesBookExist(int id);
    Task<bool> DoesOwnerExist(int id);
    Task<bool> DoesGenreExist(int id);
    Task<bool> DoesAuthorExist(int id);
    Task<GenresBookDTO> GetGenresBook(int id);
    Task<AuthorsBookDTO> GetAuthorsBook(int id);
        
    Task<int> AddBook(NewBookDTO book);
    Task AddGenreBook(int bookId, Genre genre);
    Task AddAuthorBook(int bookId, Author author);
}