using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Xamarin.Forms;

namespace MultiCurrencyWallet
{
    class HttpRequestRates
    {
                
        public static void RefreshRates(DatabaseOps db, InputValuePage page)
        {
            foreach( string code in Utils.currencyCodes){
                var uri = string.Format("http://download.finance.yahoo.com/d/quotes?f=sl1d1t1&s={0}{1}=X", "EUR", code);
                var cb = new AsyncCallback(CallHandler);
                CallWebAsync(uri, db, cb, page);
            }

        }

        private static void CallWebAsync(string uri, DatabaseOps db, AsyncCallback cb, InputValuePage page)
        {
            var request = HttpWebRequest.Create(uri);
            request.Method = "GET";
            var state = new Tuple<DatabaseOps, WebRequest, InputValuePage>(db, request, page);
            
            request.BeginGetResponse(cb, state);
        }

        private static void CallHandler(IAsyncResult ar)
        {
            var state = (Tuple<DatabaseOps, WebRequest, InputValuePage>)ar.AsyncState;
            var db = state.Item1;
            var request = state.Item2;
            var page = state.Item3;

            using (HttpWebResponse response = request.EndGetResponse(ar) as HttpWebResponse)
            {
                //Device.BeginInvokeOnMainThread(() => state.Item1.Text = "Status: " + response.StatusCode);
                if (response.StatusCode == HttpStatusCode.OK)
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var content = reader.ReadToEnd();
                        //Debug.WriteLine(content);

                        string code = content.Substring(4, 3);

                        char[] delimiterChars = { ',' };
                        string[] parts = content.Split(delimiterChars);
                        double rate = Convert.ToDouble(parts[1]);

                        Debug.WriteLine(code + " # " + parts[1] + " # " + rate);

                        db.UpdateCurrency(new Currency(code, rate));

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            page.IncrementLoadedCurrencies();
                        });

                        /*Device.BeginInvokeOnMainThread(() => {
                            state.Item2.Text = content;
                        });*/
                    }
                else
                {
                    page.ErrorLoadingCurrencies();
                }
            }
        }

    }
}
