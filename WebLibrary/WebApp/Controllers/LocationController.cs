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
using AutoMapper;
using BL.Viewmodels;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LocationController : Controller
    {
        private readonly IRepository<Location> _locationRepository;
        private readonly IMapper _mapper;

        public LocationController(IRepository<Location> locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        // GET: Location
        public async Task<IActionResult> Index()
        {
            var locations = _locationRepository.GetAll();
            return View(_mapper.Map<IEnumerable<LocationCrudVM>>(locations));
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocationCrudVM locationCrudVM)
        {
            if (ModelState.IsValid)
            {
                _locationRepository.Create(_mapper.Map<Location>(locationCrudVM));
                return RedirectToAction(nameof(Index));
            }

            return View(locationCrudVM);
        }

        // GET: Location/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var location = _locationRepository.Get(id);

            return View(_mapper.Map<LocationCrudVM>(location));
        }

        // POST: Location/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LocationCrudVM locationCrudVM)
        {
            if (id != locationCrudVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var location = _mapper.Map<Location>(locationCrudVM);
                _locationRepository.Edit(id, location);

                return RedirectToAction(nameof(Index));
            }

            return View(locationCrudVM);
        }

        // GET: Location/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var location = _locationRepository.Get(id);

            return View(_mapper.Map<LocationCrudVM>(location));
        }

        // POST: Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = _locationRepository.Get(id);

            if (location != null)
            {
                _locationRepository.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
