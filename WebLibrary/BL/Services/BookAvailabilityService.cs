using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class BookAvailabilityService
    {
        private readonly IBookLocationRepository _bookLocationRepository;
        private readonly IRepository<Book> _bookRepository;

        public BookAvailabilityService(IBookLocationRepository bookLocationRepository, IRepository<Book> bookRepository)
        {
            _bookLocationRepository = bookLocationRepository;
            _bookRepository = bookRepository;
        }

        public bool IsBookAvailable(int bookId)
        {
            return _bookLocationRepository.GetLocationsByBookId(bookId).Any();
        }

        public void UpdateBookAvailability(int bookId)
        {
            var hasAvailableLocations = _bookLocationRepository.HasAvailableLocations(bookId);
            var book = _bookRepository.Get(bookId);

            if (book != null)
            {
                book.IsAvailable = hasAvailableLocations;
                _bookRepository.Edit(bookId, book);
            }
        }
    }
}
