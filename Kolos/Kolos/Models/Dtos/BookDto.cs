namespace Kolos.Models.Dtos;

public class BookDto
{
    public int id { get; set; }
    public string title { get; set; }
    public List<string> authors { get; set; }
}