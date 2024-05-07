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
        var query = @"SELECT b.PK, b.title, STRING_AGG(a.first_name + ' ' + a.last_name, ', ') as authors
                        FROM books b
                        JOIN books_authors ab ON b.PK = ab.FK_book
                        JOIN authors a ON ab.FK_author = a.PK
                        WHERE b.PK = @id or @id = ''
                        GROUP BY b.PK, b.Title";

        await using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", id.ToString() ?? "");

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        var books = new List<BookDto>();
        while (await reader.ReadAsync())
        {
            var book = new BookDto
            {
                id = reader.GetInt32(0),
                title = reader.GetString(1),
                authors = new List<string>()
            };

            for (int i = 2; i < reader.FieldCount; i++)
            {
                if (reader.GetFieldType(i) == typeof(string))
                {
                    book.authors.Add(reader.GetString(i));
                }
            }

            books.Add(book);
        }

        return books;
    }
}