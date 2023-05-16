using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNBCurrency.Service.Models
{
    internal class Currency
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
