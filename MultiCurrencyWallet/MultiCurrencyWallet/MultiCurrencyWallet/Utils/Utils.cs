using System;
using Android.Content.Res;
using Xamarin.Forms;

namespace MultiCurrencyWallet
{
    class Utils
    {
        public static Color addColor = Color.FromRgb(70, 200, 70);
        public static Color removeColor = Color.FromRgb(200, 70, 70);
        
        public static string[] currencyCodes = { "AUD", "CAD", "CHF", "CNY", "EUR", "GBP", "INR", "JPY", "MYR", "SGD", "USD" };

        public static Color[] graphBarColors = { Color.Blue, Color.Red, Color.Green, Color.Aqua, Color.Fuchsia, Color.Black,
            Color.Lime, Color.Orange, Color.Navy, Color.Pink, Color.Silver };

        public static Currency getFavouriteCurrency(DatabaseOps db)
        {
            return db.GetCurrency(db.GetGlobal("favouriteCurrency"));
        }
    }
}
