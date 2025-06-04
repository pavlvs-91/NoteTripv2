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

    public Board(NoteTripContext context)
    {
        _context = context;
    }
    public IActionResult Show()
    {
        var result = _context.User
            .Include(u => u.Countries)
            .Select(u => new UserRankingViewModel
            {
                Login = u.Login,
                Name = u.FirstName,
                LastName = u.LastName,
                CountriesVisited = u.Countries.Count
            })
            .OrderByDescending(u => u.CountriesVisited)
            .ToList();

        return View(result);
    }


}
