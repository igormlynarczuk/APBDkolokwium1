namespace BlazorApp1.Models_and_DTOs;


public class AuthorsBookDTO
{
    public int PK { get; set; }
    public string title { get; set; } = string.Empty;
    public int OwnerId { get; set; }
    public IEnumerable<AuthorDTO> Authors { get; set; } = new List<AuthorDTO>();
}

public class AuthorDTO
{
    public int PK { get; set; }
    public string name { get; set; }
}

public class NewBookWithAuthors
{
    public int PK { get; set; }
    public string title { get; set; } = string.Empty;
    public int OwnerId { get; set; }
    public IEnumerable<Author> Authors { get; set; } = new List<Author>();
}

public class Author
{
    public int PK { get; set; }
    public string name { get; set; }
}