namespace BlazorApp1.Models_and_DTOs;

public class NewBookWithGenres
{
    public int PK { get; set; }
    public string title { get; set; } = string.Empty;
    public int OwnerId { get; set; }
    public IEnumerable<Genre> Genres { get; set; } = new List<Genre>();
}

public class Genre
{
    public int PK { get; set; }
    public string name { get; set; }
}