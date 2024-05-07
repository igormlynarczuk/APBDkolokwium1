namespace BlazorApp1.Models_and_DTOs;

public class NewBookDTO
{
    public int PK { get; set; }
    public string title { get; set; } = string.Empty;
    public int OwnerId { get; set; }
}