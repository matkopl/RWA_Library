using BL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class GenreRepository : IRepository<Genre>
    {
        private readonly WebLibraryContext _context;

        public GenreRepository(WebLibraryContext context)
        {
            _context = context;
        }

        public Genre Create(Genre value)
        {
            _context.Genres.Add(value);
            _context.SaveChanges();

            return value;
        }

        public Genre Delete(int id)
        {
            var genre = Get(id);

            _context.Genres.Remove(genre);
            _context.SaveChanges();

            return genre;
        }

        public Genre Edit(int id, Genre value)
        {
            var genre = Get(id);

            genre.Name = value.Name;

            _context.SaveChanges();

            return genre;
        }

        public Genre Get(int id)
        {
            return _context.Genres.FirstOrDefault(g => g.Id == id);
        }

        public IEnumerable<Genre> GetAll()
        {
            return _context.Genres.ToList();
        }
    }
}
