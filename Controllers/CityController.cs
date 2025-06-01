using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NoteTrip.Data;
using NoteTrip.Models;

namespace NoteTrip.Controllers
{
    public class CityController : Controller
    {
        private readonly NoteTripContext _context;

        public CityController(NoteTripContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? countryId, int? regionId)
        {
            string? userLogin = HttpContext.Session.GetString("login");

            var cities = _context.City.Include(c => c.Region).ThenInclude(r => r.Country)
                .Where(t => t.Region.Country.UserLogin == userLogin).AsQueryable();

            if (countryId.HasValue)
                cities = cities.Where(t => t.Region.CountryId == countryId.Value);

            if (regionId.HasValue)
                cities = cities.Where(t => t.RegionId == regionId.Value);

            var userCountries = _context.Country.Where(c => c.UserLogin == userLogin).ToList();

            var userRegions = _context.Region.Where(r => userCountries.Select(c => c.Id).Contains(r.CountryId)).ToList();

            ViewBag.CountryId = new SelectList(userCountries, "Id", "Name", countryId);
            ViewBag.RegionId = new SelectList(
                userRegions.Where(r => !countryId.HasValue || r.CountryId == countryId.Value),
                "Id", "Name", regionId
            );
            
            return View(await cities.ToListAsync());
        }


        // GET: City/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City
                .Include(c => c.Region)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // GET: City/Create
        public IActionResult Create()
        {
            string? userLogin = HttpContext.Session.GetString("login");
            var userRegions = _context.Region.Include(r => r.Country).Where(r => r.Country.UserLogin == userLogin).ToList();
            ViewData["RegionId"] = new SelectList(userRegions, "Id", "Name");

            return View();
        }

        // POST: City/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,RegionId")] City city)
        {
            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            string? userLogin = HttpContext.Session.GetString("login");
            var userRegions = _context.Region.Include(r => r.Country).Where(r => r.Country.UserLogin == userLogin).ToList();
            ViewData["RegionId"] = new SelectList(userRegions, "Id", "Name", city.RegionId);

            return View(city);
        }

        // GET: City/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            string? userLogin = HttpContext.Session.GetString("login");
            var userRegions = _context.Region.Include(r => r.Country).Where(r => r.Country.UserLogin == userLogin).ToList();
            ViewData["RegionId"] = new SelectList(userRegions, "Id", "Name", city.RegionId);

            return View(city);
        }

        // POST: City/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,RegionId")] City city)
        {
            if (id != city.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            string? userLogin = HttpContext.Session.GetString("login");
            var userRegions = _context.Region.Include(r => r.Country).Where(r => r.Country.UserLogin == userLogin).ToList();
            ViewData["RegionId"] = new SelectList(userRegions, "Id", "Name", city.RegionId);

            return View(city);
        }

        // GET: City/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City
                .Include(c => c.Region)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: City/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await _context.City.FindAsync(id);
            if (city != null)
            {
                _context.City.Remove(city);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(int id)
        {
            return _context.City.Any(e => e.Id == id);
        }
    }
}
