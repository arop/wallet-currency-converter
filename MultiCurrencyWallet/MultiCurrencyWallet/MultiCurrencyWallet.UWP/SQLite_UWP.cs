using System;
using System.IO;
using MultiCurrencyWallet;
using Xamarin.Forms;
using Windows.Storage;
using MultiCurrencyWallet.UWP;

[assembly: Dependency(typeof(SQLite_UWP))]

namespace MultiCurrencyWallet.UWP
{
    public class SQLite_UWP : ISQLite
    {
        public SQLite_UWP()
        {
        }
        #region ISQLite implementation
        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "MultiCurrencyWalletSQLite.db3";
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);

            var conn = new SQLite.SQLiteConnection(path);

            // Return the database connection 
            return conn;
        }
        #endregion
    }
}