using Kolos.Models.Dtos;
using Microsoft.Data.SqlClient;

namespace Kolos.Services;

public class LibraryServices : ILibraryServices
{
    private readonly IConfiguration _configuration;
    
    public LibraryServices(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<IEnumerable<BookDto>> GetBooks(int? id)
    {
        var books = new List<BookDto>();
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();
        using var command = new SqlCommand("SELECT * FROM Books", connection);
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            books.Add(new BookDto
            {
                IdBook = reader.GetInt32(0),
                Title = reader.GetString(1),
                Author = reader.GetString(2),
                ReleaseDate = reader.GetDateTime(3)
            });
        }
        return books;
    }

    
}