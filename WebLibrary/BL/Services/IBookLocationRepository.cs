using BL.Models;
using System.Collections.Generic;
using System.Linq;

namespace BL.Services
{
    public interface IBookLocationRepository
    {
        IEnumerable<Location> GetAllLocations();
        IEnumerable<Location> GetLocationsByBookId(int bookId);
        void AddBookLocation(int bookId, int locationId);
        void RemoveBookLocation(int bookId, int locationId);
        void UpdateBookLocations(int bookId, IEnumerable<int> locationIds);
    }

    public class BookLocationRepository : IBookLocationRepository
    {
        private readonly WebLibraryContext _context;

        public BookLocationRepository(WebLibraryContext context)
        {
            _context = context;
        }

        public IEnumerable<Location> GetAllLocations() => _context.Locations.ToList();

        public IEnumerable<Location> GetLocationsByBookId(int bookId)
        {
            return _context.BookLocations
                .Where(bl => bl.BookId == bookId)
                .Select(bl => bl.Location)
                .ToList();
        }

        public void AddBookLocation(int bookId, int locationId)
        {
            if (!_context.BookLocations.Any(bl => bl.BookId == bookId && bl.LocationId == locationId))
            {
                var bookLocation = new BookLocation
                {
                    BookId = bookId,
                    LocationId = locationId
                };

                _context.BookLocations.Add(bookLocation);
                _context.SaveChanges();
            }
        }

        public void RemoveBookLocation(int bookId, int locationId)
        {
            var bookLocation = _context.BookLocations
                .FirstOrDefault(bl => bl.BookId == bookId && bl.LocationId == locationId);

            if (bookLocation != null)
            {
                _context.BookLocations.Remove(bookLocation);
                _context.SaveChanges();
            }
        }

        public void UpdateBookLocations(int bookId, IEnumerable<int> locationIds)
        {
            var existingLocations = _context.BookLocations
                .Where(bl => bl.BookId == bookId)
                .ToList();
            foreach (var existingLocation in existingLocations)
            {
                if (!locationIds.Contains(existingLocation.LocationId))
                {
                    _context.BookLocations.Remove(existingLocation);
                }
            }

            foreach (var locationId in locationIds)
            {
                if (!existingLocations.Any(bl => bl.LocationId == locationId))
                {
                    _context.BookLocations.Add(new BookLocation
                    {
                        BookId = bookId,
                        LocationId = locationId
                    });
                }
            }

            _context.SaveChanges();
        }
    }
}
