using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BL.Models;
using BL.Services;
using AutoMapper;
using BL.Viewmodels;
using System.Collections.Generic;

namespace WebApp.Controllers
{
    public class BookController : Controller
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Genre> _genreRepository;
        private readonly IBookLocationRepository _bookLocationRepository;
        private readonly BookAvailabilityService _bookAvailabilityService;
        private readonly IMapper _mapper;

        public BookController(IRepository<Book> bookRepository, IRepository<Genre> genreRepository, IBookLocationRepository bookLocationRepository, BookAvailabilityService bookAvailabilityService, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _bookLocationRepository = bookLocationRepository;
            _bookAvailabilityService = bookAvailabilityService;
            _mapper = mapper;
        }

        // GET: Book
        public IActionResult Index()
        {
            var books = _bookRepository.GetAll();
            var bookVMs = _mapper.Map<IEnumerable<BookVM>>(books);
            return View(bookVMs);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_genreRepository.GetAll(), "Id", "Name");

            var vm = new CreateBookVM
            {
                Locations = _bookLocationRepository.GetAllLocations()
                    .Select(location => new LocationVM
                    {
                        Id = location.Id,
                        Name = location.Name,
                        IsChecked = false,
                        IsAvailable = _bookLocationRepository.IsLocationAvailableForBook(0, location.Id)
                    }).ToList()
            };

            return View(vm);
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateBookVM createBookVM)
        {
            if (!ModelState.IsValid)
            {
                ViewData["GenreId"] = new SelectList(_genreRepository.GetAll(), "Id", "Name", createBookVM.GenreId);
                createBookVM.Locations = _bookLocationRepository.GetAllLocations()
                    .Select(location => new LocationVM
                    {
                        Id = location.Id,
                        Name = location.Name,
                        IsChecked = false,
                        IsAvailable = _bookLocationRepository.IsLocationAvailableForBook(0, location.Id)
                    }).ToList();

                return View(createBookVM);
            }

            var book = _mapper.Map<Book>(createBookVM);
            _bookRepository.Create(book);

            foreach (var location in createBookVM.Locations.Where(l => l.IsChecked))
            {
                if (!_bookLocationRepository.IsLocationAvailableForBook(book.Id, location.Id))
                {
                    ModelState.AddModelError("", $"Location '{location.Name}' is already reserved for this book.");
                    createBookVM.Locations = _bookLocationRepository.GetAllLocations()
                        .Select(loc => new LocationVM
                        {
                            Id = loc.Id,
                            Name = loc.Name,
                            IsChecked = createBookVM.Locations.Any(l => l.Id == loc.Id && l.IsChecked),
                            IsAvailable = _bookLocationRepository.IsLocationAvailableForBook(book.Id, loc.Id)
                        }).ToList();
                    return View(createBookVM);
                }

                _bookLocationRepository.AddBookLocation(book.Id, location.Id);
            }

            _bookAvailabilityService.UpdateBookAvailability(book.Id);

            return RedirectToAction(nameof(Index));
        }

        // GET: Book/Edit/5
        public IActionResult Edit(int id)
        {
            var book = _bookRepository.Get(id);
            if (book == null)
            {
                return NotFound();
            }

            var bookVM = _mapper.Map<UpdateBookVM>(book);

            bookVM.Locations = _bookLocationRepository.GetAllLocations()
                .Select(location => new LocationVM
                {
                    Id = location.Id,
                    Name = location.Name,
                    IsChecked = _bookLocationRepository.GetLocationsByBookId(id).Any(bl => bl.Id == location.Id),
                    IsAvailable = _bookLocationRepository.IsLocationAvailableForBook(id, location.Id)
                }).ToList();

            ViewData["GenreId"] = new SelectList(_genreRepository.GetAll(), "Id", "Name", book.GenreId);

            return View(bookVM);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, UpdateBookVM updateBookVM)
        {
            if (id != updateBookVM.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewData["GenreId"] = new SelectList(_genreRepository.GetAll(), "Id", "Name", updateBookVM.GenreId);
                updateBookVM.Locations = _bookLocationRepository.GetAllLocations()
                    .Select(location => new LocationVM
                    {
                        Id = location.Id,
                        Name = location.Name,
                        IsChecked = _bookLocationRepository.GetLocationsByBookId(id).Any(bl => bl.Id == location.Id),
                        IsAvailable = _bookLocationRepository.IsLocationAvailableForBook(id, location.Id)
                    }).ToList();

                return View(updateBookVM);
            }

            var book = _mapper.Map<Book>(updateBookVM);
            _bookRepository.Edit(id, book);

            foreach (var location in updateBookVM.Locations.Where(l => l.IsChecked))
            {
                if (!_bookLocationRepository.IsLocationAvailableForBook(id, location.Id))
                {
                    ModelState.AddModelError("", $"Location '{location.Name}' is already reserved for another reservation.");
                    updateBookVM.Locations = _bookLocationRepository.GetAllLocations()
                        .Select(loc => new LocationVM
                        {
                            Id = loc.Id,
                            Name = loc.Name,
                            IsChecked = updateBookVM.Locations.Any(l => l.Id == loc.Id && l.IsChecked),
                            IsAvailable = _bookLocationRepository.IsLocationAvailableForBook(id, loc.Id)
                        }).ToList();
                    return View(updateBookVM);
                }
            }

            var selectedLocationIds = updateBookVM.Locations
                .Where(l => l.IsChecked)
                .Select(l => l.Id)
                .ToList();

            _bookLocationRepository.UpdateBookLocations(id, selectedLocationIds);

            _bookAvailabilityService.UpdateBookAvailability(id);

            return RedirectToAction(nameof(Index));
        }

        // GET: Book/Delete/5
        public IActionResult Delete(int id)
        {
            var book = _bookRepository.Get(id);
            if (book == null)
            {
                return NotFound();
            }

            var bookVM = _mapper.Map<BookVM>(book);
            return View(bookVM);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _bookRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Book/Details/5
        public IActionResult Details(int id)
        {
            var book = _bookRepository.Get(id);
            if (book == null)
            {
                return NotFound();
            }

            var bookVM = _mapper.Map<BookVM>(book);
            bookVM.AvailableLocations = _bookLocationRepository.GetLocationsByBookId(id)
                .Select(location => new LocationVM
                {
                    Id = location.Id,
                    Name = location.Name,
                    IsChecked = true,
                    IsAvailable = _bookLocationRepository.IsLocationAvailableForBook(id, location.Id)
                }).ToList();

            return View(bookVM);
        }
    }
}
