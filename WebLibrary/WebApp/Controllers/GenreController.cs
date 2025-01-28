using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using BL.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using AutoMapper;
using BL.Viewmodels;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GenreController : Controller
    {
        private readonly IRepository<Genre> _genreRepository;
        private readonly IMapper _mapper;

        public GenreController(IRepository<Genre> genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }


        // GET: Genre
        public async Task<IActionResult> Index()
        {
            var genres = _genreRepository.GetAll();
            return View(_mapper.Map<IEnumerable<GenreVM>>(genres));
        }


        // GET: Genre/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genre/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GenreVM genreVM)
        {
            if (ModelState.IsValid)
            {
                var genre = _mapper.Map<Genre>(genreVM);

                _genreRepository.Create(genre);
                return RedirectToAction(nameof(Index));
            }
            return View(genreVM);
        }

        // GET: Genre/Edit/5
        public async Task<IActionResult> Edit(int id)
        { 
            var genre = _genreRepository.Get(id);

            return View(_mapper.Map<GenreVM>(genre));
        }

        // POST: Genre/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GenreVM genreVM)
        {
            if (id != genreVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var genre = _mapper.Map<Genre>(genreVM);

                _genreRepository.Edit(id, genre);
 
                return RedirectToAction(nameof(Index));
            }

            return View(genreVM);
        }

        // GET: Genre/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var genre = _genreRepository.Get(id);

            return View(_mapper.Map<GenreVM>(genre));
        }

        // POST: Genre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genre = _genreRepository.Get(id);

            if (genre != null)
            {
                _genreRepository.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
