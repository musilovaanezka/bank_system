namespace stin.Models
{
    public class CNBCurrencyModel
    {
        public CNBCurrencyModel(string code, int amount, float exchangeRate)
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
