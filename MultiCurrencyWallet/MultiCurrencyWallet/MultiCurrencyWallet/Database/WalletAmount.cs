using SQLite;

namespace MultiCurrencyWallet.Database
{
    public class WalletAmount
    {
        [PrimaryKey]
        public string code { get; set; }
        public double amount { get; set; } // 1 EUR = rate THIS_CURRENCY

        public WalletAmount(string c, double a)
        {
            code = c;
            amount = a;
        }

        public WalletAmount()
        {
        }
    }
}
