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

namespace stin.Controllers
{
    public class AuthenticateController : Controller
    {
        //private readonly UserManager<IdentityUser> _klientManager;
        //private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthenticateController(
            //UserManager<IdentityUser> klientManager,
            IEmailService emailService, 
            IConfiguration configuration, 
            ApplicationDbContext context
            //SignInManager<IdentityUser> signInManager
            )
        {
            //_klientManager = klientManager;
            //_signInManager = signInManager;
            _emailService = emailService;
            _configuration = configuration;
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
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
                return BadRequest("Uživatel nenalezen");
            }

            //var identityUser = await _klientManager.FindByEmailAsync(klient.Username);

            //var token = await _klientManager.GenerateTwoFactorTokenAsync(identityUser, "Email");

            var url = Url.Action("LoginFromEmail");
            var url2 = HttpContext.Request.GetEncodedUrl();
            var a = @"<link>https://localhost:44354/Authenticate/LoginFromEmail?username=" + user.Username + "</link>";
            

            // TODO - vyměnit můj email za user.Username
            var message = new Message(new string[] { user.Username }, "Ověřovací kód", "Váš ověřovací link je:", a);
            
            _emailService.SendEmail(message);

            return Ok(); 
        }

        [HttpPost]
        public IActionResult LoginFromEmail([FromQuery(Name = "username")] string username)
        {
            var user = _context.Klienti
                           .Where(e => e.Username == username)
                           .FirstOrDefault(); 
            //var signIn = _signInManager.TwoFactorSignInAsync("Email", code, false, false);
           
            if (user != null)
            {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };
                    
                    var jwtToken = GetToken(authClaims);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        expiration = jwtToken.ValidTo
                    });
            }

            return NotFound($"Neplatný´ověřovací kód");

        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            
            return token;
        }


        //[HttpGet]
        //public IActionResult ToEmail()
        //{
        //    var message = new Message(new string[] {"anezka.musilova@tul.cz"}, "Test", "Hi, ...");
            
        //    _emailService.SendEmail(message);
               
        //    return Ok();
        //}

        
    }
}
