using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using stin.Models;
using Klient.Management.Service.Services;
using Klient.Management.Service.Models;

namespace stin.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly UserManager<IdentityUser> _klientManager;
        private readonly IEmailService _emailService;

        public AuthenticateController(UserManager<IdentityUser> klientManager, IEmailService emailService)
        {
            _klientManager = klientManager;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] stin.Models.Klient klient)
        {
            // existuje klient? 
            var userExist = await _klientManager.FindByEmailAsync(klient.Username);
            if (userExist == null)
            {
                return NotFound($"The user '{klient.Username}' was not found.");
            }
            return Ok();    
        }

        [HttpGet]
        public IActionResult ToEmail()
        {
            var message = new Message(new string[] {"anezka.musilova@tul.cz"}, "Test", "Hi, ...");
            
            _emailService.SendEmail(message);
               
            return Ok();
        }
    }
}
