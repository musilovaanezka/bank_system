using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using stin.Models;
using stin.Enums;
using stin.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using stin.Services;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

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
            if (TempData["name"] != null)
            {
                // vyhledání aktuálního uživatele
                var username = TempData["name"] as string;
                var user = _context.Klienti
                   .Where(e => e.Username == username)
                   .FirstOrDefault();

                ViewBag.Username = user.Username;
                ViewBag.UcetNum = user.UcetNum;

                if (TempData["message"] != null)
                {
                    ViewBag.Message = TempData["message"];
                }

                // automatická tvorba účtu v CZK, pokud se jedná o nového uživatele
                // nutno kontrolovat pouze při přihlašování, jindy je již zkontrolováno
                var ucetCZK = _context.Ucty
                    .Where(e => e.UcetNum == user.UcetNum && e.Mena == "CZK")
                    .FirstOrDefault();

                if ( ucetCZK == null ) { ucetCZK = createCZKUcet(user); }

                ViewBag.SelectMeny = createCurrencyList(user);
                ViewBag.SelectUsersMeny = createUsersCurrencyList(user);
                ViewBag.Mena = ucetCZK.Mena;
                ViewBag.Zustatek = ucetCZK.hodnota;

                return View();

            } else
            {
                return RedirectToAction("Login", "Authenticate");
            }

        }
        public IActionResult UsersPageRedirect(string ucetNumber, string mena)
        {
            string number = ucetNumber;
            var user = _context.Klienti
                  .Where(e => e.UcetNum == number)
                  .FirstOrDefault();


            if (user != null)
            {
                var ucet = _context.Ucty
                    .Where(e => e.UcetNum == user.UcetNum && e.Mena == mena)
                    .FirstOrDefault();

                ViewBag.Username = user.Username;
                ViewBag.UcetNum = user.UcetNum;
                ViewBag.SelectMeny = createCurrencyList(user);
                ViewBag.SelectUsersMeny = createUsersCurrencyList(user);
                ViewBag.Mena = ucet.Mena;
                ViewBag.Zustatek = ucet.hodnota;

                if (TempData["message"] != null)
                {
                    ViewBag.Message = TempData["message"];
                }

                return View("UsersPage");
            }

            return RedirectToAction("Login", "Authenticate");

        }

        private Ucet createCZKUcet(stin.Models.Klient User)
        {
            Ucet novy = new Ucet
            {
                UcetNum = User.UcetNum,
                Mena = "CZK",
                hodnota = 0
            };

            _context.Ucty.Add(novy);
            _context.SaveChanges();

            return novy;
        }

        private List<SelectListItem> createCurrencyList(stin.Models.Klient user)
        {
            // seznam měn pro select 
            Currencies[] currencies = (Currencies[])Enum.GetValues(typeof(Currencies));

            List<SelectListItem> currencyList = new List<SelectListItem>();

            foreach (Currencies curr in currencies)
            {
                var mena = Enum.GetName(typeof(Currencies), curr);
                var ucet = _context.Ucty
                   .Where(e => e.UcetNum == user.UcetNum && e.Mena == mena)
                   .FirstOrDefault();
                if (ucet == null)
                {
                    currencyList.Add(new SelectListItem { Text = curr.ToString(), Value = curr.ToString() });
                }
            }

            return currencyList;
        }

        private List<SelectListItem> createUsersCurrencyList(stin.Models.Klient user)
        {
            // seznam měn pro select 
            Currencies[] currencies = (Currencies[])Enum.GetValues(typeof(Currencies));

            List<SelectListItem> currencyList = new List<SelectListItem>();

            foreach (Currencies curr in currencies)
            {
                var mena = Enum.GetName(typeof(Currencies), curr);
                var ucet = _context.Ucty
                   .Where(e => e.UcetNum == user.UcetNum && e.Mena == mena)
                   .FirstOrDefault();
                if (ucet != null)
                {
                    currencyList.Add(new SelectListItem { Text = curr.ToString(), Value = curr.ToString() });
                }
            }

            return currencyList;
        }

        [HttpPost]
        public IActionResult ChangeMena(string ucetNum, string Mena)
        {
            return RedirectToAction("UsersPageRedirect", new { ucetNumber = ucetNum, mena = Mena });
        }

        [HttpPost]
        public IActionResult Vklad(string ucetNum, string Mena)
        {
            Random rnd = new Random();

            // náhodný výběr měny pro vklad
            Currencies[] currencies = (Currencies[])Enum.GetValues(typeof(Currencies));
            int indexCurr = rnd.Next(0, currencies.Length);
            string vkladMena = Enum.GetName(typeof(Currencies), indexCurr);

            // kontrola existence účtu daného čísla v dané měně 
            var ucet = _context.Ucty
                .Where(e => e.UcetNum == ucetNum && e.Mena == vkladMena)
                .FirstOrDefault();

            // připsání vkladu na daný účet 
            if (ucet != null)
            {

                //náhodný výběr částky pro vklad
                int vkladCastka = rnd.Next(10, 100001);

                ucet.hodnota += vkladCastka;

                ViewBag.VkladMena = vkladMena;
            
                ViewBag.VkladCastka = vkladCastka;

                _context.Ucty.Update(ucet);
                _context.SaveChanges();

                TempData["message"] = "Vklad " + vkladCastka + " " + vkladMena +  " byl úspěšně proveden";

            } else
            {
                TempData["message"] = "Vklad nemohl být proveden, protože nevedete účet v měně " + vkladMena;
            } 

            return RedirectToAction("UsersPageRedirect", new { ucetNumber = ucetNum, mena = Mena });
        }

        [HttpPost]
        public IActionResult Platba(string ucetNum, string Mena)
        {
            Random rnd = new Random();

            // náhodný výběr měny pro platbu
            Currencies[] currencies = (Currencies[])Enum.GetValues(typeof(Currencies));
            int indexCurr = rnd.Next(0, currencies.Length);
            string platbaMena = Enum.GetName(typeof(Currencies), indexCurr);

            // kontrola existence účtu daného čísla v dané měně 
            var ucet = _context.Ucty
                .Where(e => e.UcetNum == ucetNum && e.Mena == platbaMena)
                .FirstOrDefault();


            if (ucet != null)
            {

                //náhodný výběr částky pro vklad
                int platbaCastka = rnd.Next(10, 100001);

                if (ucet.hodnota -  platbaCastka < 0)
                {
                    var ucetCZK = _context.Ucty
                        .Where(e => e.UcetNum == ucetNum && e.Mena == platbaMena)
                        .FirstOrDefault();

                    var prevedena = ToCZK(platbaCastka, platbaMena);

                    if (ucetCZK.hodnota - prevedena < 0)
                    {
                        TempData["message"] = "Nemáte dostatečný zůstatek na CZK účtu";
                    } else
                    {
                        ucetCZK.hodnota -= prevedena;

                        _context.SaveChanges();

                        TempData["message"] = "Platba byla úspěšně provedena";
                    }

                } else
                {
                    ucet.hodnota -= platbaCastka;

                    _context.SaveChanges();

                    TempData["message"] = "Platba byla úspěšně provedena";
                }

            }
            else
            {
                TempData["message"] = "Platba nemohla být provedena, protože nevedete účet v dané měně";
            }
            return RedirectToAction("UsersPageRedirect", new { ucetNumber = ucetNum, mena = Mena });
        }

        private int ToCZK(int castka, string mena)
        {
            var CNBRecord = CNBCurrencyList.currencies.FirstOrDefault(o => o.Code == mena);
            if (CNBRecord == null)
            {
                return castka;
            }
            float kurz = CNBRecord.ExchangeRate;
            float toAmout = castka / CNBRecord.Amount;
            return (int)(kurz * toAmout);
        }


        [HttpPost]
        public IActionResult AddUcet(string currency, string ucetNum, string Mena)
        {

            Ucet novy = new Ucet
            {
                UcetNum = ucetNum,
                Mena = currency,
                hodnota = 0
            };

            _context.Ucty.Add(novy);
            _context.SaveChanges();

            TempData["message"] = "Účet v nové měně vytvořen";
            return RedirectToAction("UsersPageRedirect", new { ucetNumber = ucetNum, mena = Mena }); 
        } 

    }
}
