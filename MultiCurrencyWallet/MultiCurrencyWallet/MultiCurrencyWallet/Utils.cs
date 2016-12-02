using Xamarin.Forms;

namespace MultiCurrencyWallet
{
    class Utils
    {
        public static Color addColor = Color.FromRgb(70, 200, 70);
        public static Color removeColor = Color.FromRgb(200, 70, 70);

        public static Currency favoriteCurrency; // load this when app loads

        public static string[] currencyCodes = { "AUD", "CAD", "CHF", "CNY", "EUR", "GBP", "INR", "JPY", "MYR", "SGD", "USD" };
    }
}
