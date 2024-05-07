using Kolos.Models.Dtos;

namespace Kolos.Services;

public interface ILibraryServices
{
    Task<IEnumerable<BookDto>> GetBooks(int? id);
}