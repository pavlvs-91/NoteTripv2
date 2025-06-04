using Microsoft.AspNetCore.Mvc;
namespace MvcMovie.Controllers;

using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using NoteTrip.Data;

public class Api : Controller
{
    private readonly NoteTripContext _context;

    public Api(NoteTripContext context)
    {
        _context = context;
    }

    [HttpPost("/api/login")]
    public IActionResult Login([FromBody] JsonElement data)
    {
        string login = data.GetProperty("login").GetString();
        string password = data.GetProperty("password").GetString();
        bool check = CheckPass(login, password);
        if (check) HttpContext.Session.SetString("login", login);
        return check
            ? Ok(new { message = "Login successful", login = login })
            : Unauthorized(new { message = "Invalid login or password" });
    }

    private bool CheckPass(string login, string password)
    {
        return _context.User.Any(u => u.Login == login && u.Password == Hash.get(password));
    }

    [HttpGet("/api/countries")]
    
    public IActionResult GetCountries()
    {
        if (HttpContext.Session.GetString("login") == null)
        {
            return Unauthorized(new { message = "You must be logged in to access this resource." });
        }
        var countries = _context.Country.Where(u => u.UserLogin.Equals(HttpContext.Session.GetString("login").ToString())).ToList();
        Console.WriteLine("Countries: " + JsonSerializer.Serialize(countries));
        return Ok(new { message = "Login successful", countries = countries });
    }
}
