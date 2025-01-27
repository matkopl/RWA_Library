using BL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IReservationRepository
    {
        IEnumerable<Reservation> GetReservationsByUserId(int userId); 
        void AddReservation(Reservation reservation); 
        void DeleteReservation(int reservationId); 
        bool IsBookAvailableAtLocation(int bookId, int locationId);
        Reservation Get(int id);
    }

    public class ReservationRepository : IReservationRepository
    {
        private readonly WebLibraryContext _context;

        public ReservationRepository(WebLibraryContext context)
        {
            _context = context;
        }

        public IEnumerable<Reservation> GetReservationsByUserId(int userId)
        {
            return _context.Reservations
                .Include(r => r.Book)  
                .ThenInclude(b => b.Genre) 
                .Include(r => r.Location)  
                .Where(r => r.UserId == userId)
                .ToList();
        }

        public void AddReservation(Reservation reservation)
        {
            if (!_context.Reservations.Any(r =>
           r.BookId == reservation.BookId &&
           r.LocationId == reservation.LocationId &&
           r.UserId == reservation.UserId))
            {
                _context.Reservations.Add(reservation);

                var location = _context.BookLocations.FirstOrDefault(bl =>
                    bl.BookId == reservation.BookId && bl.LocationId == reservation.LocationId);

                if (location != null)
                {
                    _context.BookLocations.Remove(location);
                }

                bool hasLocations = _context.BookLocations.Any(bl => bl.BookId == reservation.BookId);
                var book = _context.Books.FirstOrDefault(b => b.Id == reservation.BookId);

                if (book != null)
                {
                    book.IsAvailable = hasLocations;
                }

                _context.SaveChanges();
            }
        }

        public void DeleteReservation(int reservationId)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == reservationId);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);

                _context.BookLocations.Add(new BookLocation
                {
                    BookId = reservation.BookId,
                    LocationId = reservation.LocationId
                });

                var book = _context.Books.FirstOrDefault(b => b.Id == reservation.BookId);
                if (book != null)
                {
                    book.IsAvailable = true;
                }

                _context.SaveChanges();
            }
        }

        public bool IsBookAvailableAtLocation(int bookId, int locationId)
        {
            return _context.BookLocations.Any(bl => bl.BookId == bookId && bl.LocationId == locationId);
        }

        public Reservation Get(int id)
        {
            return _context.Reservations
            .Include(r => r.Book)
            .ThenInclude(b => b.Genre)
            .Include(r => r.Location)
            .FirstOrDefault(r => r.Id == id);
        }
    }
}
