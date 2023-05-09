using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using stin.Models;
using stin.Data;

namespace stin.Controllers
{
    public class HomePageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomePageController(
            ApplicationDbContext context
        )
        {
            _context = context;
        }


        public IActionResult UsersPage()
        {
            var username = TempData["name"] as string;
            var user = _context.Klienti
               .Where(e => e.Username == username)
               .FirstOrDefault();
            ViewBag.Klient = user;
            return View();
        }


    }
}
