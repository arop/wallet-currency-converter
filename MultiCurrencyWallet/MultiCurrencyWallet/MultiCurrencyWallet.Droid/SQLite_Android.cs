using System;
using MultiCurrencyWallet;
using Xamarin.Forms;
using MultiCurrencyWallet.Droid;
using System.IO;

[assembly: Dependency(typeof(SQLite_Android))]

namespace MultiCurrencyWallet.Droid
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android()
        {
        }

        #region ISQLite implementation

        public SQLite.SQLiteConnection GetConnection()
        {
            var fileName = "MultiCurrencyWalletSQLite.db3";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, fileName);
            
            var connection = new SQLite.SQLiteConnection(path);

            return connection;
        }

        #endregion
    }
}