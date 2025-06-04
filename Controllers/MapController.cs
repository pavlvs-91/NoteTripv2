using Microsoft.AspNetCore.Mvc;
namespace MvcMovie.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using NoteTrip.Data;
using NoteTrip.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using IsoCountry = ISO3166.Country; 
public class Map : Controller
{
    private readonly NoteTripContext _context;

    public Map(NoteTripContext context)
    {
        _context = context;
    }
    public IActionResult Show()
    {
        string login = HttpContext.Session.GetString("login");
        var countryMap = ISO3166.Country.List.GroupBy(c => c.Name).ToDictionary(g => g.Key, g => g.First().TwoLetterCode);
        var countryStats = _context.Country
                .Where(c => c.UserLogin == login)
                .Include(c => c.Regions)
                .ThenInclude(r => r.Cities)
                .ThenInclude(city => city.TouristAttractions)
                .Select(c => new
                {
                    CountryName = c.Name,
                    VisitedCount = c.Regions
                        .SelectMany(r => r.Cities)
                        .SelectMany(city => city.TouristAttractions)
                        .Count(attr => attr.Visited)
                })
                .ToList();
        var visitedCountries = new List<string>();
        foreach (var country in countryStats)
        {
            if (country.VisitedCount > 0 && countryMap.ContainsKey(country.CountryName))
            {
                visitedCountries.Add(countryMap[country.CountryName]);
            }
        }
        var numOfCountries = ISO3166.Country.List.Count(); 
        var numOfVisitedCountries = visitedCountries.Count();
        var percentageVisited = numOfVisitedCountries * 100 / numOfCountries;
        ViewData["VisitedCountriesJson"] = JsonSerializer.Serialize(visitedCountries);
        ViewData["numOfCountries"] = numOfCountries;
        ViewData["numOfVisitedCountries"] = numOfVisitedCountries;
        ViewData["percentageVisited"] = percentageVisited;
        return View();
    }

}
