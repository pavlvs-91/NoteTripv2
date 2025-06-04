using Microsoft.AspNetCore.Mvc;
namespace MvcMovie.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using NoteTrip.Data;
using NoteTrip.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using IsoCountry = ISO3166.Country;
using Microsoft.CodeAnalysis.Text;

public class Board : Controller
{
    private readonly NoteTripContext _context;
    private static readonly Dictionary<string, string> countryMap = ISO3166.Country.List
        .GroupBy(c => c.Name)
        .ToDictionary(g => g.Key, g => g.First().TwoLetterCode);

    public Board(NoteTripContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Show()
    {
        var users = await _context.User.ToListAsync();

        var result = users.Select(u => new UserRankingViewModel
        {
            Login = u.Login,
            Name = u.FirstName,
            LastName = u.LastName,
            CountriesVisited = GetStatsPerLogin(u.Login).Count,
        })
        .OrderByDescending(u => u.CountriesVisited)
        .ToList();

        return View(result);
    }

    public List<string> GetStatsPerLogin(string login)
    {
        if (string.IsNullOrEmpty(login))
            return new List<string>();

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

        return visitedCountries;
    }
}
