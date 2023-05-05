using Microsoft.AspNetCore.Mvc;
using stin.Models;
using System.Diagnostics;

namespace stin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            getCNBData(new DateTime(2023,5,3));
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<List<Currency>> getCNBData(DateTime date)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt;date=" + date.ToString("dd.MM.yyyy")); //03.05.2023
            var pindik = await response.Content.ReadAsStringAsync();
            return parseData(pindik, new List<string> { "EUR", "USD" });
        }

        private List<Currency> parseData(string data, List<string> wanted)
        {
            List<string> records = new List<string>();
            records = data.Split("\n").ToList();
            records.RemoveRange(0, 2);
            records.RemoveAt(records.Count - 1);

            List<Currency> currencyList = new List<Currency>();
            foreach (var record in records)
            {
                string[] values = record.Split("|");
                currencyList.Add(new Currency(values[3], int.Parse(values[2]), float.Parse(values[4])));
            }
            return currencyList.Where((curr) => wanted.Contains(curr.Code)).ToList();
        }
    }

    public class Currency
    {

        public Currency(string code, int amount, float exchangeRate)
        {
            Code = code;
            Amount = amount;
            ExchangeRate = exchangeRate;
        }
        public string Code { get; set; }

        public int Amount { get; set; }

        public float ExchangeRate { get; set; }
    }
}