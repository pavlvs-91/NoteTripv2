using Microsoft.AspNetCore.Mvc;
namespace MvcMovie.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using NoteTrip.Data;
using NoteTrip.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public class Map : Controller
{
    private readonly NoteTripContext _context;

    public Map(NoteTripContext context)
    {
        _context = context;
    }
    public IActionResult Show()
    {
        
        var visitedCountries = new List<string> { "PL", "DE", "FR" };
        ViewData["VisitedCountriesJson"] = JsonSerializer.Serialize(visitedCountries);
        Console.Write("here");
        return View();
    }

}
