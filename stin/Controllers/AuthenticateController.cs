using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using stin.Models;
using Klient.Management.Service.Services;
using Klient.Management.Service.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using stin.Data;
using Microsoft.AspNetCore.Http.Extensions;
using stin.Services;

namespace stin.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthenticateController(
            IEmailService emailService, 
            IConfiguration configuration, 
            ApplicationDbContext context
            )
        {
            _emailService = emailService;
            _configuration = configuration;
            _context = context;
        }

        public IActionResult Login()
        {
            ViewBag.Message = TempData["message"];
            return View();
        }

        public IActionResult EmailOdeslan()
        {
            return View();
        }

        public IActionResult RedirectToHomePage(string username) {
            TempData["name"] = username;
            return RedirectToAction("UsersPage", "HomePage");
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // ověření přihlašovacích údajů 
            var user = _context.Klienti
                .Where(e => e.Username == username && e.Password == password)
                .FirstOrDefault();

            if (user == null)
            {
                TempData["message"] = "Uživatel nenalezen";
                return RedirectToAction("Login");
            }

            // tvorba autentizačního kódu pro link do emailu 
            var code = new AuthenticationCodeService().getAuthenticationCode(username);

            _context.AutenticationCodes.Add(code);
            _context.SaveChanges();

            var strCode = code.Code;

            var url = Url.Action(action: "LoginFromEmail", controller: "Authenticate", values: new {code = strCode, name = username}, protocol: "https", host: "localhost:44354");
            

            // TODO - vyměnit můj email za user.Username
            var message = new Message(new string[] { user.Username }, "Ověřovací kód", "Váš ověřovací link je:", url);
            
            _emailService.SendEmail(message);

            return RedirectToAction("EmailOdeslan"); 
        }

        [HttpGet]
        public IActionResult LoginFromEmail(string code, string name) // místo name bude code
        {
            var objCode = _context.AutenticationCodes
                .Where(e => e.Code == code && e.Username == name)
                .FirstOrDefault();

            if (objCode == null) {
                TempData["message"] = "Neplatný kód přihlášení";
                return RedirectToAction("Login");
            }
            
            if (objCode.EndDateTime < DateTime.Now)
            {
                TempData["message"] = "Platnost přihlášení vypršela";
                return RedirectToAction("Login");
            }

            var user = _context.Klienti
                .Where(e => e.Username == name)
                .FirstOrDefault();

            if (user == null)
            {
                TempData["message"] = "Tento uživatel neexistuje";
                return RedirectToAction("Login");
            }

                return RedirectToAction("RedirectToHomePage", new {username = name});

        }
        
    }
}
