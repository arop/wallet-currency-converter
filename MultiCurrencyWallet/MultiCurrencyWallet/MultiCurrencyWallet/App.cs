using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MultiCurrencyWallet
{
    public class App : Application
    {

        DatabaseOps db = new DatabaseOps();

        public App()
        {

            // EM FASE DE TESTES, HARD CODED
            db.UpdateCurrency(new Currency() { code = "EUR", rate = 1 });
            db.UpdateCurrency(new Currency() { code = "GBP", rate = 0.85 });
            db.UpdateCurrency(new Currency() { code = "USD", rate = 1.05 });
            db.UpdateCurrency(new Currency() { code = "CAD", rate = 1.3 });

            Utils.favoriteCurrency = db.GetCurrencies().ElementAt(1);

            // The root page of your application
            var content = new InputValuePage(db);
            content.Title = "Add Money";
            MainPage = new NavigationPage(content);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
           
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        /*private void OnButton_Clicked(object sender, EventArgs e)
        {
            var uri = string.Format("http://download.finance.yahoo.com/d/quotes?f=sl1d1t1&s={0}{1}=X", entFrom.Text, entTo.Text);
            var cb = new AsyncCallback(CallHandler);
            CallWebAsync(uri, lab1, lab2, cb);
        }

        private void CallWebAsync(string uri, Label status, Label response, AsyncCallback cb)
        {
            var request = HttpWebRequest.Create(uri);
            request.Method = "GET";
            var state = new Tuple<Label, Label, WebRequest>(status, response, request);

            request.BeginGetResponse(cb, state);
        }

        private void CallHandler(IAsyncResult ar)
        {
            var state = (Tuple<Label, Label, WebRequest>)ar.AsyncState;
            var request = state.Item3;

            using (HttpWebResponse response = request.EndGetResponse(ar) as HttpWebResponse)
            {
                Device.BeginInvokeOnMainThread(() => state.Item1.Text = "Status: " + response.StatusCode);
                if (response.StatusCode == HttpStatusCode.OK)
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var content = reader.ReadToEnd();
                        Device.BeginInvokeOnMainThread(() => state.Item2.Text = content);
                    }
            }
        }*/
        }
    }
