using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class LocationRepository : IRepository<Location>
    {
        private readonly WebLibraryContext _context;

        public LocationRepository(WebLibraryContext context)
        {
            _context = context;
        }

        public Location Create(Location value)
        {
            _context.Locations.Add(value);
            _context.SaveChanges();

            return value;
        }

        public Location Delete(int id)
        {
            var location = Get(id);

            _context.Locations.Remove(location);
            _context.SaveChanges();

            return location;
        }

        public Location Edit(int id, Location value)
        {
            var location = Get(id);

            location.Name = value.Name;

            _context.Locations.Update(location);
            _context.SaveChanges();
            return location;
        }

        public Location Get(int id)
        {
            return _context.Locations.FirstOrDefault(l => l.Id == id);
        }

        public IEnumerable<Location> GetAll()
        {
            return _context.Locations.ToList().OrderBy(l => l.Name);
        }
    }
}
