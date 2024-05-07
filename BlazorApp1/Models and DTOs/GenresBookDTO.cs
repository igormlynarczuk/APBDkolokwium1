namespace BlazorApp1.Models_and_DTOs;

public class GenresBookDTO
{
    public int PK { get; set; }
    public string title { get; set; } = string.Empty;
    public int OwnerId { get; set; }
    public IEnumerable<GenreDTO> Genres { get; set; } = new List<GenreDTO>();
}

public class GenreDTO
{
    public int PK { get; set; }
    public String nam { get; set; }
}


