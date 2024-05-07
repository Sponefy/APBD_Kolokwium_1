using Kolos.Models.Dtos;
using Kolos.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kolos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly ILibraryServices _service;
    
    public BooksController(ILibraryServices service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetBooks(int? id)
    {
        var books = await _service.GetBooks(id);
        if (!books.Any())
        {
            return NotFound("No books found");
        }

        return Ok(books);
    }
}