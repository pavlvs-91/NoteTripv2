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
    public class TouristAttractionController : Controller
    {
        private readonly NoteTripContext _context;

        public TouristAttractionController(NoteTripContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? countryId, int? regionId, int? cityId)
        {
            string? userLogin = HttpContext.Session.GetString("login");

            var attractions = _context.TouristAttraction.Include(t => t.City).ThenInclude(c => c.Region).ThenInclude(r => r.Country)
                .Where(t => t.City.Region.Country.UserLogin == userLogin).AsQueryable();

            if (countryId.HasValue)
                attractions = attractions.Where(t => t.City.Region.CountryId == countryId.Value);

            if (regionId.HasValue)
                attractions = attractions.Where(t => t.City.RegionId == regionId.Value);

            if (cityId.HasValue)
                attractions = attractions.Where(t => t.CityId == cityId.Value);

            var userCountries = _context.Country.Where(c => c.UserLogin == userLogin).ToList();

            var userRegions = _context.Region.Where(r => userCountries.Select(c => c.Id).Contains(r.CountryId)).ToList();

            var userCities = _context.City.Where(c => userRegions.Select(r => r.Id).Contains(c.RegionId)).ToList();

            ViewBag.CountryId = new SelectList(userCountries, "Id", "Name", countryId);
            ViewBag.RegionId = new SelectList(
                userRegions.Where(r => !countryId.HasValue || r.CountryId == countryId.Value),
                "Id", "Name", regionId
            );
            ViewBag.CityId = new SelectList(
                userCities.Where(c =>
                    (!regionId.HasValue || c.RegionId == regionId.Value) &&
                    (!countryId.HasValue || c.Region.CountryId == countryId.Value)
                ),
                "Id", "Name", cityId
            );

            return View(await attractions.ToListAsync());
        }

        // GET: TouristAttraction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var touristAttraction = await _context.TouristAttraction
                .Include(t => t.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (touristAttraction == null)
            {
                return NotFound();
            }

            return View(touristAttraction);
        }

        // GET: TouristAttraction/Create
        public IActionResult Create()
        {
            string? userLogin = HttpContext.Session.GetString("login");
            var userCities = _context.City.Include(t => t.Region).ThenInclude(r => r.Country).Where(r => r.Region.Country.UserLogin == userLogin).ToList();
            ViewData["CityId"] = new SelectList(userCities, "Id", "Name");

            return View();
        }

        // POST: TouristAttraction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Category,Description,Price,Visited,Rate,CityId")] TouristAttraction touristAttraction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(touristAttraction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            string? userLogin = HttpContext.Session.GetString("login");
            var userCities = _context.City.Include(t => t.Region).ThenInclude(r => r.Country).Where(r => r.Region.Country.UserLogin == userLogin).ToList();
            ViewData["CityId"] = new SelectList(userCities, "Id", "Name", touristAttraction.CityId);

            return View(touristAttraction);
        }

        // GET: TouristAttraction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var touristAttraction = await _context.TouristAttraction.FindAsync(id);
            if (touristAttraction == null)
            {
                return NotFound();
            }
            string? userLogin = HttpContext.Session.GetString("login");
            var userCities = _context.City.Include(t => t.Region).ThenInclude(r => r.Country).Where(r => r.Region.Country.UserLogin == userLogin).ToList();
            ViewData["CityId"] = new SelectList(userCities, "Id", "Name", touristAttraction.CityId);
            
            return View(touristAttraction);
        }

        // POST: TouristAttraction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category,Description,Price,Visited,Rate,CityId")] TouristAttraction touristAttraction)
        {
            if (id != touristAttraction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(touristAttraction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TouristAttractionExists(touristAttraction.Id))
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
            var userCities = _context.City.Include(t => t.Region).ThenInclude(r => r.Country).Where(r => r.Region.Country.UserLogin == userLogin).ToList();
            ViewData["CityId"] = new SelectList(userCities, "Id", "Name", touristAttraction.CityId);

            return View(touristAttraction);
        }

        // GET: TouristAttraction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var touristAttraction = await _context.TouristAttraction
                .Include(t => t.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (touristAttraction == null)
            {
                return NotFound();
            }

            return View(touristAttraction);
        }

        // POST: TouristAttraction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var touristAttraction = await _context.TouristAttraction.FindAsync(id);
            if (touristAttraction != null)
            {
                _context.TouristAttraction.Remove(touristAttraction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TouristAttractionExists(int id)
        {
            return _context.TouristAttraction.Any(e => e.Id == id);
        }
    }
}
