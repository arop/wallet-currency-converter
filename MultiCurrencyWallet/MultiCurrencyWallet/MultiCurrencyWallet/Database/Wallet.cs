using System;
using System.Collections.Generic;

namespace MultiCurrencyWallet
{
    public class Wallet
    {
        private Dictionary<String, double> balances;

        public Wallet()
        {
            balances = new Dictionary<string, double>();
        }

        public Dictionary<string, double> Balances
        {
            get
            {
                return balances;
            }

            set
            {
                balances = value;
            }
        }

        public void AddAmount(string code, double amount)
        {
            double currentValue = 0.0;
            if (balances.TryGetValue(code, out currentValue))
                balances[code] = currentValue + amount;
            else balances.Add(code, amount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="amount"></param>
        /// <returns>New value for this currency. Negative if not enough money.</returns>
        public double RemoveAmount(string code, double amount)
        {
            double currentValue = 0.0;
            if (balances.TryGetValue(code, out currentValue))
            {
                if (currentValue - amount >= 0.0)
                {
                    balances[code] = currentValue - amount;
                    if (balances[code] == 0)
                    {
                        balances.Remove(code);
                        return 0;
                    }
                    return balances[code];
                }
            }
            return -1;
        }

        public double GetTotal(string code, DatabaseOps db)
        {
            double total = 0.0;
            foreach (KeyValuePair<string, double> entry in balances)
            {
                // do something with entry.Value or entry.Key
                if (entry.Key != code)
                {
                    total += Currency.ConvertAmount(db, entry.Value, entry.Key, code);
                }
                else total += entry.Value;
            }
            return total;
        }

    }
}
