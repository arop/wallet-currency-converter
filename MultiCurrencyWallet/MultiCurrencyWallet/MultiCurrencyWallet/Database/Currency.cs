﻿
using SQLite;

namespace MultiCurrencyWallet
{
    public class Currency
    {
        [PrimaryKey]
        public string code { get; set; }
        public double rate { get; set; } // 1 EUR = rate THIS_CURRENCY

        public Currency(string c, double conv)
        {
            code = c;
            rate = conv;
        }

        public Currency()
        {
        }

        public double getRate(Currency c)
        {
            return this.rate / c.rate; // 1 c = return THIS
        }

        public static double ConvertAmount(DatabaseOps db, double amount, string codeFrom, string codeTo)
        {
            double rate = db.GetCurrency(codeTo).rate / db.GetCurrency(codeFrom).rate;
            return amount * rate;
        }
    }
}
