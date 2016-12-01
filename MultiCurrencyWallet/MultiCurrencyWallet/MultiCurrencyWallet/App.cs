using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MultiCurrencyWallet
{
    public class App : Application
    {
        public App()
        {

            DatabaseOps db = new DatabaseOps();

            // EM FASE DE TESTES, CORRER ISTO SÓ UMA VEZ. DEPOIS COMENTAM-SE OS INSERTS. SENAO DÁ EXCEPTION (UNIQUE code)
            db.UpdateCurrency(new Currency() { code = "EUR", rate = 1 });
            /*db.InsertCurrency(new Currency() { code = "GBP", rate = 0.85 });
            db.InsertCurrency(new Currency() { code = "USD", rate = 1.05 });
            db.InsertCurrency(new Currency() { code = "CAD", rate = 1.3 });*/

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
    }
}
