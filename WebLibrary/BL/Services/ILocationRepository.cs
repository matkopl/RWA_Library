using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface ILocationRepository
    {
        IEnumerable<Location> GetLocations();
        IEnumerable<Location> GetLocationsByBookId(int bookId);
        Location GetLocationById(int id);
        void AddBookLocation(int bookId, int locationId);
        void RemoveBookLocation(int bookId, int locationId);
    }
    public class LocationRepository : ILocationRepository
    {
        private readonly WebLibraryContext _context;

        public LocationRepository(WebLibraryContext context)
        {
            _context = context;
        }

        public void AddBookLocation(int bookId, int locationId)
        {
            var bookLocation = new BookLocation
            {
                BookId = bookId,
                LocationId = locationId
            };

            _context.BookLocations.Add(bookLocation);
            _context.SaveChanges();
        }

        public Location GetLocationById(int id) => _context.Locations.FirstOrDefault(l => l.Id == id);

        public IEnumerable<Location> GetLocations() => _context.Locations.ToList();

        public IEnumerable<Location> GetLocationsByBookId(int bookId)
        {
            throw new NotImplementedException();
        }

        public void RemoveBookLocation(int bookId, int locationId)
        {
            var bookLocation = _context.BookLocations.FirstOrDefault(b => b.BookId == bookId && b.LocationId == locationId);

            if (bookLocation != null)
            {
                _context.BookLocations.Remove(bookLocation);
                _context.SaveChanges(); 
            }
        }
    }
}
