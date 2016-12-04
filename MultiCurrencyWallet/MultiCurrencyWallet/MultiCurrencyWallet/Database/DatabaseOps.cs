
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using SQLite;
using MultiCurrencyWallet.Database;

namespace MultiCurrencyWallet
{
    public class DatabaseOps
    {

        public SQLiteConnection database;
        public static object locker = new object();

        public DatabaseOps()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<Currency>();
            database.CreateTable<WalletAmount>();
            database.CreateTable<Global>();
        }

        
        ////////////// CURRENCY ///////////////////
        public IEnumerable<Currency> GetCurrencies()
        {
            lock (locker)
            {
                return (from i in database.Table<Currency>() select i).ToList();
            }
        }

        /*public IEnumerable<Currency> GetItemsNotDone()
        {
            lock (locker)
            {
                return database.Query<Currency>("SELECT * FROM Currency WHERE rate = 1");
            }
        }*/

        public Currency GetCurrency(string code)
        {
            lock (locker)
            {
                return database.Table<Currency>().FirstOrDefault(x => x.code == code);
            }
        }

        public int DeleteCurrency(int id)
        {
            lock (locker)
            {
                return database.Delete<Currency>(id);
            }
        }


        public void InsertCurrency(Currency c)
        {
            lock (locker)
            {
                database.Insert(c);
            }
        }

        /**
         * Updates item, or inserts if it doesn't exist.
         */
        public void UpdateCurrency(Currency c)
        {
            lock (locker)
            {
                if (database.Update(c) == 0)
                    database.Insert(c);
            }
        }


        ////////////// WALLET AMOUNT ///////////////////
        public IEnumerable<WalletAmount> GetWalletAmounts()
        {
            lock (locker)
            {
                return (from i in database.Table<WalletAmount>() select i).ToList();
            }
        }
       

        public WalletAmount GetWalletAmount(string code)
        {
            lock (locker)
            {
                return database.Table<WalletAmount>().FirstOrDefault(x => x.code == code);
            }
        }

        public int DeleteWalletAmount(int id)
        {
            lock (locker)
            {
                return database.Delete<WalletAmount>(id);
            }
        }


        public void InsertWalletAmount(WalletAmount c)
        {
            lock (locker)
            {
                database.Insert(c);
            }
        }

        /**
         * Updates item, or inserts if it doesn't exist.
         */
        public void UpdateWalletAmount(WalletAmount c)
        {
            lock (locker)
            {
                if (database.Update(c) == 0)
                    database.Insert(c);
            }
        }


        ///////////////////// GLOBALS ////////////////////
        public string GetGlobal(string k)
        {
            lock (locker)
            {
                var result = database.Table<Global>().FirstOrDefault(x => x.key == k);
                if (result != null)
                    return result.value;
                return null;
            }
        }

        public void SetGlobal(string k, string v)
        {
            Global g = new Global(k, v);

            lock (locker)
            {
                if (database.Update(g) == 0)
                    database.Insert(g);
            }
        }

    }
}
