using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteTrip.Data;
using NoteTrip.Models;
using System.Linq;
using System.Threading.Tasks;

namespace NoteTrip.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly NoteTripContext _context;

        public StatisticsController(NoteTripContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string? userLogin = HttpContext.Session.GetString("login");

            // Kraje z liczbą odwiedzonych atrakcji
            var countryStats = await _context.Country
                .Where(c => c.UserLogin == userLogin)
                .Include(c => c.Regions)
                    .ThenInclude(r => r.Cities)
                        .ThenInclude(city => city.TouristAttractions)
                .Select(c => new
                {
                    CountryName = c.Name,
                    VisitedCount = c.Regions
                        .SelectMany(r => r.Cities)
                        .SelectMany(city => city.TouristAttractions)
                        .Count(attr => attr.Visited),
                    TotalAttractions = c.Regions
                        .SelectMany(r => r.Cities)
                        .SelectMany(city => city.TouristAttractions)
                        .Count()
                })
                .OrderByDescending(c => c.VisitedCount)
                .ToListAsync();

            var cityStats = await _context.City
                .Include(city => city.TouristAttractions)
                .Include(city => city.Region)
                    .ThenInclude(r => r.Country)
                .Where(city => city.Region.Country.UserLogin == userLogin)
                .Select(city => new
                {
                    CityName = city.Name,
                    RegionName = city.Region.Name,
                    CountryName = city.Region.Country.Name,
                    Total = city.TouristAttractions.Count(),
                    Visited = city.TouristAttractions.Count(a => a.Visited)
                })
                .ToListAsync();

            var model = new StatisticsViewModel
            {
                CountryStats = countryStats
                    .Select(c => new CountryStat
                    {
                        CountryName = c.CountryName,
                        VisitedCount = c.VisitedCount,
                        TotalAttractions = c.TotalAttractions
                    })
                    .ToList(),
                CityStats = cityStats
                    .Select(cs => new CityStat
                    {
                        CityName = cs.CityName,
                        RegionName = cs.RegionName,
                        CountryName = cs.CountryName,
                        TotalAttractions = cs.Total,
                        VisitedAttractions = cs.Visited
                    })
                    .ToList()
            };

            var visitedCountries = _context.TouristAttraction
                .Include(a => a.City)
                    .ThenInclude(c => c.Region)
                        .ThenInclude(r => r.Country)
                .Where(a => a.City.Region.Country.UserLogin == userLogin)
                .Select(a => a.City.Region.Country.Id)
                .Distinct()
                .Count();

            var visitedCities = _context.TouristAttraction
                .Include(a => a.City)
                .Where(a => a.City.Region.Country.UserLogin == userLogin)
                .Select(a => a.City.Id)
                .Distinct()
                .Count();

            var topAttractions = _context.TouristAttraction
                .Include(a => a.City)
                    .ThenInclude(c => c.Region)
                        .ThenInclude(r => r.Country)
                .Where(a => a.City.Region.Country.UserLogin == userLogin)
                .OrderByDescending(a => a.Rate)
                .Take(5)
                .ToList();

            ViewBag.VisitedCountriesCount = visitedCountries;
            ViewBag.VisitedCitiesCount = visitedCities;
            ViewBag.TopAttractions = topAttractions;

            return View(model);
        }
    }
}
