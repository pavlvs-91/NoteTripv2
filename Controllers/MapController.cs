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
        
        var countryMap = ISO3166.Country.List.GroupBy(c => c.Name).ToDictionary(g => g.Key, g => g.First().TwoLetterCode);
        var visitedCountries = _context.Country
            .Where(c => c.UserLogin == HttpContext.Session.GetString("login"))
            .Select(c => countryMap.ContainsKey(c.Name) ? countryMap[c.Name] : null
            )
            .ToList();

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
