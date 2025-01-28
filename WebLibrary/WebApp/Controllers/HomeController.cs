using AutoMapper;
using BL.Models;
using BL.Services;
using BL.Viewmodels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IRepository<Genre> _genreRepository;
    private readonly IRepository<Book> _bookRepository;
    private readonly IMapper _mapper;

    public HomeController(IHttpClientFactory httpClientFactory, IRepository<Genre> genreRepository, IRepository<Book> bookRepository, IMapper mapper)
    {
        _httpClientFactory = httpClientFactory;
        _genreRepository = genreRepository;
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var genres = _genreRepository.GetAll();
        ViewData["Genres"] = genres;

        var books = _bookRepository.GetAll();
        var booksVM = _mapper.Map<IEnumerable<BookIndexVM>>(books).OrderBy(b => b.Name);

        return View(booksVM);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
