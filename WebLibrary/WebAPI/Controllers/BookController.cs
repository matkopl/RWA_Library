using AutoMapper;
using BL.DTO;
using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BookController : ControllerBase
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogRepository _logRepository;

        public BookController(IRepository<Book> bookRepository, IMapper mapper, ILogRepository logRepository)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logRepository = logRepository;
        }

        // GET: api/<BookController>
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var books = _bookRepository.GetAll();
                var bookDtos = _mapper.Map<IEnumerable<BookDto>>(books);

                if (bookDtos.Any())
                {
                    _logRepository.AddLog("Successfully retrieved all books", 3);
                    return Ok(bookDtos);
                }

                _logRepository.AddLog("No books found", 1);
                return NotFound();
            }
            catch (Exception e)
            {
                _logRepository.AddLog("An error occurred while retrieving all books", 5);
                return StatusCode(500, e.Message);
            }
        }

        // GET api/<BookController>/5
        [HttpGet("Get/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var book = _bookRepository.Get(id);

                if (book != null)
                {
                    var bookDto = _mapper.Map<BookDto>(book);

                    _logRepository.AddLog($"Retrieved book id={id}", 3);
                    return Ok(bookDto);
                }

                _logRepository.AddLog($"Book with id={id} not found", 1);
                return NotFound();
            }
            catch (Exception e)
            {
                _logRepository.AddLog($"An error occurred while retrieving book id={id}", 5);
                return StatusCode(500, e.Message);
            }
        }

        // POST api/<BookController>
        [HttpPost("Create")]
        public IActionResult CreateBook([FromBody] BookDto value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var book = _mapper.Map<Book>(value);
                var createdBook = _bookRepository.Create(book);

                var bookDto = _mapper.Map<BookDto>(createdBook);
                _logRepository.AddLog($"Succesfully created book {bookDto.Name} with id={bookDto.Id}", 3);
                return Ok(book);
                //return CreatedAtAction(nameof(GetBookById), new { id = bookDto.Id }, bookDto);
            }
            catch (Exception e)
            {
                _logRepository.AddLog($"An error occurred while creating book", 5);
                return StatusCode(500, e.Message);
            }
        }

        // PUT api/<BookController>/5
        [HttpPut("Update/{id}")]
        public IActionResult Put(int id, [FromBody] BookDto value)
        {
            if (!ModelState.IsValid)
            {

                return NotFound();
            }

            try
            {
                var book = _mapper.Map<Book>(value);
                var updatedBook = _bookRepository.Edit(id, book);

                var bookDto = _mapper.Map<BookDto>(updatedBook);
                _logRepository.AddLog($"Succesfully updated book with id={id}", 3);
                return Ok(bookDto);
            }
            catch (Exception e)
            {
                _logRepository.AddLog($"An error occurred while updating book id={id}", 5);
                return StatusCode(500, e.Message);
            }
        }

        // DELETE api/<BookController>/5
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var book = _bookRepository.Get(id);

                if (book != null)
                {
                    _bookRepository.Delete(id);
                    _logRepository.AddLog($"Successfully deleted book id={id}", 3);
                    return Ok();
                }

                _logRepository.AddLog($"Book with id={id} not found", 1);
                return NotFound();
            }
            catch (Exception e)
            {
                _logRepository.AddLog($"An error occurred while deleting book id={id}", 5);
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string name, [FromQuery] int? genreId, [FromQuery] int page = 1, [FromQuery] int count = 10)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name parameter is required");
            }

            try
            {
                var bookRepo = (BookRepository)_bookRepository;
                var books = bookRepo.SearchBooks(name, genreId, page, count);

                if (!books.Any())
                {
                    _logRepository.AddLog("No books found", 2);
                    return NotFound();
                }

                _logRepository.AddLog($"Successfully listed all books with the search term: {name}", 2);
                return Ok(books);
            }
            catch (Exception e)
            {
                _logRepository.AddLog("An error occurred while searching", 3);
                return StatusCode(500, e.Message);
            }
        }
    }
}
