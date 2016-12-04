using MultiCurrencyWallet.Database;
using MultiCurrencyWallet.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MultiCurrencyWallet
{
    public class InputValuePage : ContentPage
    {
        private Picker actionPicker, currencyPicker;
        private Label entryLabel, currencyRateLabel;
        private Entry valueEntry;
        private Button button;
        private DatabaseOps db;
        private Currency selectedCurrency;
        private Currency favouriteCurrency;

        private Wallet wallet;



        public InputValuePage(DatabaseOps db, Wallet w)
        {
            this.Title = "Add/Remove Money";

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
            
            foreach (Currency c in db.GetCurrencies())
            {
                currencyPicker.Items.Add(c.code);
            }
            currencyPicker.SelectedIndex = 0;
            selectedCurrency = db.GetCurrencies().ElementAt(0);
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

            for(int i = 0; i < 7; i++)
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            grid.Children.Add(actionPicker, 1, 0);
            grid.Children.Add(entryLabel, 1, 1);
            grid.Children.Add(valueEntry, 1, 2);
            grid.Children.Add(currencyPicker, 1, 3);
            grid.Children.Add(currencyRateLabel, 1, 4);
            grid.Children.Add(button, 1, 5);

            Content = new StackLayout{
                Children = { grid }
            };

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
            this.currencyRateLabel.Text = string.Format("1 {0} = {1} {2}", favouriteCurrency.code,
                    Math.Round(db.GetCurrency(selectedCurrency.code).getRate(favouriteCurrency), 5),
                    selectedCurrency.code);
        }


        void OnButtonClicked(object sender, EventArgs e)
        {
            string code = selectedCurrency.code;
            try
            {
                double amount = Convert.ToDouble(valueEntry.Text);
                valueEntry.BackgroundColor = Color.Default;
                if (button.Text == "Add")
                {
                    wallet.AddAmount(code, amount);
                }
                else wallet.RemoveAmount(code, amount);

                db.UpdateWalletAmount(new WalletAmount(code, wallet.Balances[code]));
                
            } catch
            {
                valueEntry.BackgroundColor = Color.Red;
            }
        }
        
        async private void ShowBalancePage()
        {
            BalancePage newPage = new BalancePage(wallet, db);
            await Navigation.PushAsync(newPage, true);
        }

        async private void ShowSetFavouriteCurrencyPage()
        {
            SetFavouriteCurrencyPage newPage = new SetFavouriteCurrencyPage(db);
            await Navigation.PushAsync(newPage, true);
        }        
    }
}
