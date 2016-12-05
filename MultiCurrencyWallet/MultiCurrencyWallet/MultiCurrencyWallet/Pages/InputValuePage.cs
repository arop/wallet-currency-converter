using MultiCurrencyWallet.Database;
using MultiCurrencyWallet.Pages;
using System;
using System.Linq;

using Xamarin.Forms;

namespace MultiCurrencyWallet
{
    public class InputValuePage : ContentPage
    {
        private Picker actionPicker, currencyPicker;
        private Label entryLabel, currencyRateLabel, errorLabel;
        private Entry valueEntry;
        private Button button;

        private DatabaseOps db;
        private Currency selectedCurrency;
        private Currency favouriteCurrency;

        private int loadedCurrencies;
        private bool errorLoadingCurrencies;

        private Wallet wallet;
        private int numberOfCurrencies;

        static object locker = new object();
        private ProgressBar progressBar;

        public InputValuePage(DatabaseOps db, Wallet w)
        {
            Title = "Add/Remove Money";

            this.db = db;
            wallet = w;

            favouriteCurrency = Utils.getFavouriteCurrency(db);

            //////////////////// TOOLBAR /////////////////////
            var balancePageButton = new ToolbarItem
            {
                Text = "Balance",
                Command = new Command(this.ShowBalancePage)
            };

            if (Device.OS == TargetPlatform.Windows)
            {
                balancePageButton.Order = ToolbarItemOrder.Secondary;
            }

            ToolbarItems.Add(balancePageButton);

            var setFavouriteCurrencyButton = new ToolbarItem
            {
                Text = "Favourite Currency",
                Command = new Command(this.ShowSetFavouriteCurrencyPage)
            };

            setFavouriteCurrencyButton.Order = ToolbarItemOrder.Secondary;
            ToolbarItems.Add(setFavouriteCurrencyButton);

            var showRatesPageButton = new ToolbarItem
            {
                Text = "Rates",
                Command = new Command(this.ShowRatesPage)
            };

            showRatesPageButton.Order = ToolbarItemOrder.Secondary;
            ToolbarItems.Add(showRatesPageButton);


            var updateCurrenciesButton = new ToolbarItem
            {
                Text = "Update Currencies",
                Command = new Command(this.UpdateCurrencies)
            };

            updateCurrenciesButton.Order = ToolbarItemOrder.Secondary;
            ToolbarItems.Add(updateCurrenciesButton);

            /////////////////// PROGRESS BAR //////////////////

            progressBar = new ProgressBar
            {
                Progress = 0,
            };           


            ////////////////// ACTION PICKER //////////////////
            actionPicker = new Picker
            {
                Title = "Action",
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            actionPicker.Items.Add("Add");
            actionPicker.Items.Add("Remove");
            actionPicker.SelectedIndex = 0;
            actionPicker.SelectedIndexChanged += actionChanged;

            ////////////////// VALUE INPUT //////////////////
            entryLabel = new Label()
            {
                Text = "Amount:"
            };
            valueEntry = new Entry()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Placeholder = "Amount",
                Keyboard = Keyboard.Numeric
            };

            ////////////////// CURRENCY PICKER //////////////////
            currencyPicker = new Picker
            {
                Title = "Currency",
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            int favouriteIndex = 0;
            numberOfCurrencies = 0;
            foreach (Currency c in db.GetCurrencies())
            {
                currencyPicker.Items.Add(c.code);
                if (c.code == favouriteCurrency.code)
                    favouriteIndex = numberOfCurrencies;
                numberOfCurrencies++;
            }
            currencyPicker.SelectedIndex = favouriteIndex;
            selectedCurrency = db.GetCurrencies().ElementAt(favouriteIndex);
            currencyPicker.SelectedIndexChanged += currencyChanged;

            ////////////////// CURRENT CURRENCY RATE //////////////////

            currencyRateLabel = new Label();
            updateCurrencyRateLabelText();

            ////////////////// BUTTON //////////////////
            button = new Button()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "Add",
                BackgroundColor = Utils.addColor
            };
            button.Clicked += OnButtonClicked;

            ////////////////// ERROR LABEL //////////////////
            errorLabel = new Label()
            {
                Text = "",
                TextColor = Color.Red,
                HorizontalTextAlignment = TextAlignment.Center
            };

            ////////////////// GRID //////////////////
            var grid = new Grid()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            grid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }
            };

            for (int i = 0; i < 7; i++)
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            grid.Children.Add(errorLabel, 1, 0);
            grid.Children.Add(actionPicker, 1, 1);
            grid.Children.Add(entryLabel, 1, 2);
            grid.Children.Add(valueEntry, 1, 3);
            grid.Children.Add(currencyPicker, 1, 4);
            grid.Children.Add(currencyRateLabel, 1, 5);
            grid.Children.Add(button, 1, 6);

            Content = new StackLayout
            {
                Children = { progressBar,  grid }
            };

        }

        public void ErrorLoadingCurrencies()
        {
            lock (locker)
            {
                errorLabel.Text = "Not every currency was updated!";
            }
        }

        public void IncrementLoadedCurrencies()
        {
            lock (locker)
            {
                if (errorLoadingCurrencies)
                    return;
                loadedCurrencies++;
                //errorLabel.Text = "Loading Currencies (" + loadedCurrencies + "/" + numberOfCurrencies + ")";

                var progress = loadedCurrencies / numberOfCurrencies;
                // animate the progression to 80%, in 250ms
                progressBar.ProgressTo(progress, 250, Easing.Linear);

                if (currencyPicker.Items.Count == loadedCurrencies)
                {
                    errorLabel.Text = "All currencies were updated!";
                }
            }
        }

        public void UpdateCurrencies()
        {
            lock (locker)
            {
                errorLoadingCurrencies = false;
                loadedCurrencies = 0;
                //errorLabel.Text = "Loading Currencies (" + loadedCurrencies + "/" + numberOfCurrencies + ")";
                errorLabel.Text = "";
                progressBar.ProgressTo(0, 0, Easing.Linear);
                HttpRequestRates.RefreshRates(db, this);
            }
        }


        private void actionChanged(object sender, EventArgs e)
        {
            if(((Picker)sender).SelectedIndex == 0)
            {
                button.Text = "Add";
                button.BackgroundColor = Utils.addColor;
            }
            else
            {
                button.Text = "Remove";
                button.BackgroundColor = Utils.removeColor;
            }
        }

        private void currencyChanged(object sender, EventArgs e)
        {
            selectedCurrency = db.GetCurrencies().ElementAt(((Picker)sender).SelectedIndex);
            updateCurrencyRateLabelText();
        }

        private void updateCurrencyRateLabelText()
        {
            currencyRateLabel.Text = string.Format("1 {0} = {1} {2}", favouriteCurrency.code,
                    Math.Round(db.GetCurrency(selectedCurrency.code).getRate(favouriteCurrency), 5),
                    selectedCurrency.code);
        }

        public void updateFavouriteCurrency()
        {
            favouriteCurrency = Utils.getFavouriteCurrency(db);
            updateCurrencyRateLabelText();
        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            string code = selectedCurrency.code;
            try
            {
                errorLabel.Text = "";
                double amount = Convert.ToDouble(valueEntry.Text);
                valueEntry.BackgroundColor = Color.Default;
                if (button.Text == "Add")
                {
                    wallet.AddAmount(code, amount);
                }
                else if(!wallet.RemoveAmount(code, amount)) // not enough money to remove
                {
                    SetAmountError("Not enough money!");
                }

                db.UpdateWalletAmount(new WalletAmount(code, wallet.Balances[code]));

                valueEntry.Text = "";
            } catch
            {
                SetAmountError("Invalid value!");
            }
        }

        private void SetAmountError(string errorMessage)
        {
            /*if (Device.OS == TargetPlatform.Android)
            {
                valueEntry.BackgroundColor = Color.Red.MultiplyAlpha(0.5);
            }
            else // UWP
            {
                valueEntry.BackgroundColor = Color.Red;
            }*/
            valueEntry.PlaceholderColor = Color.Red;
            valueEntry.Focus();
            errorLabel.Text = errorMessage;
        }
        
        async private void ShowBalancePage()
        {
            BalancePage newPage = new BalancePage(wallet, db);
            await Navigation.PushAsync(newPage, true);
        }

        async private void ShowSetFavouriteCurrencyPage()
        {
            SetFavouriteCurrencyPage newPage = new SetFavouriteCurrencyPage(db,this);
            await Navigation.PushAsync(newPage, true);
        }

        async private void ShowRatesPage()
        {
            RatesPage newPage = new RatesPage(db);
            await Navigation.PushAsync(newPage, true);
        }
    }
}
