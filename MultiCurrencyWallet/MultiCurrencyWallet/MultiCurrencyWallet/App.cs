using MultiCurrencyWallet.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MultiCurrencyWallet
{
    public class App : Application
    {

        DatabaseOps db = new DatabaseOps();
        Wallet wallet = new Wallet();

        public App()
        {
            if(db.GetCurrency(Utils.currencyCodes[0]) == null) // db not initialized
            {
                // initialize db with default rate (-1)
                foreach (string cod in Utils.currencyCodes)
                {
                    db.UpdateCurrency(new Currency() { code = cod, rate = -1.0 });
                }
            }

            Utils.favoriteCurrency = db.GetCurrencies().ElementAt(4);

            foreach(WalletAmount wa in db.GetWalletAmounts())
            {
                wallet.AddAmount(wa.code, wa.amount);
            }


            // The root page of your application
            var content = new InputValuePage(db,wallet);
            content.Title = "Add Money";
            MainPage = new NavigationPage(content);

            HttpRequestRates.RefreshRates(db);
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
        
        }
    }
