using BL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class BookRepository : IRepository<Book>
    {
        private readonly WebLibraryContext _context;

        public BookRepository(WebLibraryContext context)
        {
            _context = context;
        }

        public Book Create(Book value)
        {
            _context.Books.Add(value);
            _context.SaveChanges();

            return value;
        }

        public Book Delete(int id)
        {
            var book = Get(id);

            _context.Books.Remove(book); 
            _context.SaveChanges();

            return book;
        }

        public Book Edit(int id, Book value)
        {
            var book = Get(id);

            book.Name = value.Name;
            book.Description = value.Description;
            book.Author = value.Author;
            book.IsAvailable = value.IsAvailable;
            book.GenreId = value.GenreId;

            _context.SaveChanges();

            return book;
        }

        public Book Get(int id)
        {
            return _context.Books
                .Include(b => b.Genre)
                .FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Book> SearchBooks(string searchTerm, int? genreId, int page, int count)
        {
            var query = _context.Books.Include(b => b.Genre).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(b => b.Name.Contains(searchTerm) || b.Author.Contains(searchTerm));
            }

            if (genreId.HasValue)
            {
                query = query.Where(b => b.GenreId == genreId);
            }

            return query
                .Skip((page - 1) * count)
                .Take(count)
                .OrderBy(b => b.Name ?? "")
                .ToList();
        }

        public IEnumerable<Book> GetAll()
        {
            return _context.Books.Include(b => b.Genre);
        }

        public void AddBookLocation(BookLocation bookLocation)
        {
            _context.BookLocations.Add(bookLocation);
            _context.SaveChanges();
        }
    }
}
