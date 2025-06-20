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
    public class CountryController : Controller
    {
        private readonly NoteTripContext _context;

        public CountryController(NoteTripContext context)
        {
            _context = context;
        }

        // GET: Country
        public async Task<IActionResult> Index()
        {
            var noteTripContext = _context.Country.Include(c => c.User);
            return View(await noteTripContext.ToListAsync());
        }

        // GET: Country/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Country/Create
        public IActionResult Create()
        {
            ViewData["UserLogin"] = new SelectList(_context.User, "Login", "Login");
            var countries = ISO3166.Country.List.Select(c => c.Name).OrderBy(n => n).ToList();
            var continents = new List<string>
            {
                "Africa", "Asia", "Europe", "North America", "South America", "Oceania", "Antarctica"
            };
            ViewBag.CountryList = new SelectList(countries);
            ViewBag.ContinentList = new SelectList(continents);
            return View();
        }

        // POST: Country/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Continent,Language,Currency,Capital")] Country country)
        {
            string? userLogin = HttpContext.Session.GetString("login");
            country.UserLogin = userLogin;

            if (ModelState.IsValid)
            {
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Country/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            ViewData["UserLogin"] = new SelectList(_context.User, "Login", "Login", country.UserLogin);
            var countries = ISO3166.Country.List.Select(c => c.Name).OrderBy(n => n).ToList();
            var continents = new List<string>
            {
                "Africa", "Asia", "Europe", "North America", "South America", "Oceania", "Antarctica"
            };
            ViewBag.CountryList = new SelectList(countries);
            ViewBag.ContinentList = new SelectList(continents);
            return View(country);
        }

        // POST: Country/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Continent,Language,Currency,Capital")] Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            string? userLogin = HttpContext.Session.GetString("login");
            country.UserLogin = userLogin;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.Id))
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
            ViewData["UserLogin"] = new SelectList(_context.User, "Login", "Login", country.UserLogin);
            return View(country);
        }

        // GET: Country/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Country/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var country = await _context.Country.FindAsync(id);
            if (country != null)
            {
                _context.Country.Remove(country);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(int id)
        {
            return _context.Country.Any(e => e.Id == id);
        }
    }
}
