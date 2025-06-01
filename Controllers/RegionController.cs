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
    public class RegionController : Controller
    {
        private readonly NoteTripContext _context;

        public RegionController(NoteTripContext context)
        {
            _context = context;
        }

        // GET: Region
        public async Task<IActionResult> Index(int? countryId)
        {
            // var noteTripContext = _context.Region.Include(r => r.Country);
            // return View(await noteTripContext.ToListAsync());

            string? userLogin = HttpContext.Session.GetString("login");
            var regions = _context.Region.Include(r => r.Country).Where(t => t.Country.UserLogin == userLogin).AsQueryable();

            if (countryId.HasValue)
                regions = regions.Where(t => t.CountryId == countryId.Value);

            var userCountries = _context.Country.Where(c => c.UserLogin == userLogin).ToList();
            ViewBag.CountryId = new SelectList(userCountries, "Id", "Name", countryId);

            // var userCountries = await _context.Country.Where(c => c.UserLogin == userLogin).ToListAsync();
            // ViewData["CountryId"] = new SelectList(userCountries, "Id", "Name");

            // var regionsQuery = _context.Region.Include(r => r.Country).Where(r => r.Country.UserLogin == userLogin);

            // if (countryId.HasValue)
            // {
            //     regionsQuery = regionsQuery.Where(r => r.CountryId == countryId.Value);
            // }

            // var regions = await regionsQuery.ToListAsync();
            // return View(regions);
            return View(await regions.ToListAsync());
        }

        // GET: Region/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _context.Region
                .Include(r => r.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (region == null)
            {
                return NotFound();
            }

            return View(region);
        }

        // GET: Region/Create
        public IActionResult Create()
        {
            string? userLogin = HttpContext.Session.GetString("login");
            var userCountries = _context.Country.Where(c => c.UserLogin == userLogin).ToList();

            ViewData["CountryId"] = new SelectList(userCountries, "Id", "Name");
            return View();
        }

        // POST: Region/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,Description,CountryId")] Region region)
        {
            if (ModelState.IsValid)
            {
                _context.Add(region);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "UserLogin", region.CountryId);
            return View(region);
        }

        // GET: Region/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _context.Region.FindAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            string? userLogin = HttpContext.Session.GetString("login");
            var userCountries = _context.Country.Where(c => c.UserLogin == userLogin).ToList();
            ViewData["CountryId"] = new SelectList(userCountries, "Id", "Name", region.CountryId);
            
            return View(region);
        }

        // POST: Region/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,Description,CountryId")] Region region)
        {
            if (id != region.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(region);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegionExists(region.Id))
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
            ViewData["CountryId"] = new SelectList(_context.Country, "Id", "UserLogin", region.CountryId);
            return View(region);
        }

        // GET: Region/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _context.Region
                .Include(r => r.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (region == null)
            {
                return NotFound();
            }

            return View(region);
        }

        // POST: Region/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var region = await _context.Region.FindAsync(id);
            if (region != null)
            {
                _context.Region.Remove(region);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegionExists(int id)
        {
            return _context.Region.Any(e => e.Id == id);
        }
    }
}
