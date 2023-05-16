using stin.Controllers;
using System.Diagnostics.CodeAnalysis;

namespace stin.Services
{

    public static class CNBCurrencyList
    {
        public static List<Currency> currencies = new List<Currency>();
    }

    public class CNBCurrencyService
    {
        public static async void getCNBCurrWhileRun()
        {
            var now = DateTime.Now;
            if(CNBCurrencyList.currencies.Count == 0)
            {
                getCNBDataByDate(now);
            }

            var nextRunTime = new DateTime(now.Year, now.Month, now.Day, 14, 0, 0);
            if (now > nextRunTime)
            {
                nextRunTime = nextRunTime.AddDays(1);
            }

            TimeSpan initDelay = nextRunTime - now;

            Timer timer = new Timer(getCNBData, null, initDelay, TimeSpan.FromDays(1));
        }

        private static async void getCNBDataByDate(DateTime date)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt;date=" + date.ToString("dd.MM.yyyy")); //03.05.2023
            var pindik = await response.Content.ReadAsStringAsync();
            CNBCurrencyList.currencies = parseData(pindik, Enum.GetNames(typeof(stin.Enums.Currencies)).ToList());
        }

        private static async void getCNBData(object? state)
        {
            var date = DateTime.Now;
            List<Currency> previousCurrencies = CNBCurrencyList.currencies;

            // Try to update the data for 3 sets of 5 minutes, then 3 sets of 10 minutes, then 2 sets of 30 minutes
            for (int i = 0; i < 8; i++)
            {
                if (!previousCurrencies.SequenceEqual(CNBCurrencyList.currencies))
                {
                    // Data has been updated, so break out of the loop
                    break;
                }

                // Wait for a set amount of time before trying to update the data again
                int waitTime = (i < 3) ? 5 : ((i < 6) ? 10 : 30);
                await Task.Delay(TimeSpan.FromMinutes(waitTime));

                // Try to update the data
                HttpClient client = new HttpClient();
                var response = await client.GetAsync("https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt;date=" + date.ToString("dd.MM.yyyy")); //03.05.2023
                var pindik = await response.Content.ReadAsStringAsync();
                CNBCurrencyList.currencies = parseData(pindik, Enum.GetNames(typeof(stin.Enums.Currencies)).ToList());
            }
        }

        private static List<Currency> parseData(string data, List<string> wanted)
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
}