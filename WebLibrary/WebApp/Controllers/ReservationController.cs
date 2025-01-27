using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BL.Models;
using AutoMapper;
using BL.Services;
using BL.Viewmodels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize(Roles = "User")]
    public class ReservationController : Controller
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRepository<Book> _bookRepository;
        private readonly IBookLocationRepository _bookLocationRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IMapper _mapper;
        private readonly BookAvailabilityService _bookAvailabilityService;

        public ReservationController(IReservationRepository reservationRepository, IRepository<Book> bookRepository, IBookLocationRepository bookLocationRepository, IRepository<Genre> genreRepository, IMapper mapper, BookAvailabilityService bookAvailabilityService)
        {
            _reservationRepository = reservationRepository;
            _bookRepository = bookRepository;
            _bookLocationRepository = bookLocationRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
            _bookAvailabilityService = bookAvailabilityService;
        }

        // GET: Reservation
        public IActionResult Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userId = int.Parse(userIdClaim.Value);
            var reservations = _reservationRepository.GetReservationsByUserId(userId);

            var reservationVMs = reservations.Select(r => new ReservationVM
            {
                Id = r.Id,
                Book = r.Book.Name,
                Genre = r.Book.Genre.Name,
                Location = r.Location.Name,
                ReservationDate = r.ReservationDate
            }).ToList();

            return View(reservationVMs);
        }

        // GET: Reservation/Create
        public IActionResult Create(int bookId)
        {
            var book = _bookRepository.Get(bookId);
            if (book == null)
            {
                return NotFound("Book does not exist.");
            }

            if (!_bookAvailabilityService.IsBookAvailable(bookId))
            {
                return BadRequest("Book is not available for reservation.");
            }

            var locations = _bookLocationRepository.GetLocationsByBookId(bookId)
                .Select(location => new LocationVM
                {
                    Id = location.Id,
                    Name = location.Name
                }).ToList();

            var vm = new CreateReservationVM
            {
                BookId = book.Id,
                Book = book.Name,
                Genre = book.Genre.Name,
                Locations = locations
            };

            return View(vm);
        }

        // POST: Reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateReservationVM createReservationVM)
        {
            if (!ModelState.IsValid)
            {
                var book = _bookRepository.Get(createReservationVM.BookId);
                if (book == null)
                {
                    return NotFound("Book does not exist.");
                }

                createReservationVM.Book = book.Name;
                createReservationVM.Genre = book.Genre.Name;
                createReservationVM.Locations = _bookLocationRepository.GetLocationsByBookId(createReservationVM.BookId)
                    .Select(location => new LocationVM
                    {
                        Id = location.Id,
                        Name = location.Name
                    }).ToList();

                return View(createReservationVM);
            }

            var bookValidation = _bookRepository.Get(createReservationVM.BookId);
            if (bookValidation == null)
            {
                return NotFound("Book does not exist.");
            }

            var locationValidation = _bookLocationRepository.GetLocationsByBookId(createReservationVM.BookId)
                .FirstOrDefault(l => l.Id == createReservationVM.LocationId);
            if (locationValidation == null)
            {
                return NotFound("Location does not exist.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userId = int.Parse(userIdClaim.Value);

            var reservation = new Reservation
            {
                BookId = createReservationVM.BookId,
                LocationId = createReservationVM.LocationId,
                UserId = userId,
                ReservationDate = DateTime.Now
            };

            _reservationRepository.AddReservation(reservation);

            _bookAvailabilityService.UpdateBookAvailability(createReservationVM.BookId);

            return RedirectToAction(nameof(Index));

        }

        // POST: Reservation/Delete/5
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userId = int.Parse(userIdClaim.Value);
            var reservation = _reservationRepository.Get(id);

            if (reservation == null || reservation.UserId != userId)
            {
                return NotFound();
            }

            _reservationRepository.DeleteReservation(id);

            _bookAvailabilityService.UpdateBookAvailability(reservation.BookId);

            return RedirectToAction("Index");
        }
    }
}
