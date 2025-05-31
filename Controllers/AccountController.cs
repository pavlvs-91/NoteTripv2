using Microsoft.AspNetCore.Mvc;
namespace MvcMovie.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using NoteTrip.Data;
using NoteTrip.Models;
using Microsoft.EntityFrameworkCore;


public class Account : Controller
{
    private readonly NoteTripContext _context;

    public Account(NoteTripContext context)
    {
        _context = context;
    }
    public IActionResult Create()
    {
        if (HttpContext.Session.GetString("login") != null)
        {
            return Redirect("/");
        }
        else return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("Login,FirstName,LastName,Password,Email")] User user)
    {
        if (ModelState.IsValid)
         {
            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Login == user.Login);

            if (existingUser != null)
            {
                TempData["info"] = "Requested login already exists!";
                return RedirectToAction("Create", "Account");
            }
            user.Password = Hash.get(user.Password);
            _context.Add(user);
            await _context.SaveChangesAsync();
            return  Redirect("/");
        }
        TempData["info"] = "Something went wrong!";
        return RedirectToAction("Create", "Account");

    }

}
