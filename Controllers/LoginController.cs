using Microsoft.AspNetCore.Mvc;
namespace MvcMovie.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using NoteTrip.Data;

public class Login : Controller
{
    private readonly NoteTripContext _context;

    public Login(NoteTripContext context)
    {
        _context = context;
    }

    public IActionResult Form()
    {
        if (HttpContext.Session.GetString("login") != null)
        {
            return Redirect("/");
        }
        else return View();
    }
    [HttpPost]
    public IActionResult Form(IFormCollection form)
    {
        string password = form["password"].ToString();
        string login = form["login"].ToString();
        if (login.Equals("admin") && password.Equals("admin"))
        {
            HttpContext.Session.SetString("role", "admin");
            HttpContext.Session.SetString("login", "admin");
            return RedirectToAction("Index", "User");
        }
        bool check = CheckPass(login, password);
        if (check)
        {
            HttpContext.Session.SetString("login", login);
            return Redirect("/Home/Index");
        }
        else
        {
            TempData["info"] = "Invalid login or password!";
            return RedirectToAction("Form", "Login");
        }

    }
    private bool CheckPass(string login, string password)
    {
        return _context.User.Any(u => u.Login == login && u.Password == Hash.get(password));
    }

    [HttpPost]
    public IActionResult LogOut(IFormCollection form)
    {
        HttpContext.Session.Clear();
        return Redirect("/Login/Form");
    }


}
