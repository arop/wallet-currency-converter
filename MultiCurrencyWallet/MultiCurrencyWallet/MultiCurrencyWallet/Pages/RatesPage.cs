using System;
using System.Linq;

using Xamarin.Forms;

namespace MultiCurrencyWallet.Pages
{
    public class RatesPage : ContentPage
    {
        private DatabaseOps db;
        private Picker currencyPicker;
        private Currency selectedCurrency;
        private Grid mainGrid, grid;

        public RatesPage(DatabaseOps db)
        {
            Title = "Currency Rates";
            this.db = db;
            selectedCurrency = Utils.getFavouriteCurrency(db);

            ////////////////// GRID //////////////////
            mainGrid = new Grid()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            mainGrid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }
            };

            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            setPicker();
            updateGrid();
            mainGrid.Children.Add(currencyPicker, 1, 0);
            mainGrid.Children.Add(grid, 1, 1);
                    
            Content = new StackLayout
            {
                Children = { mainGrid }
            };
        }

        private void setPicker()
        {
            ////////////////// CURRENCY PICKER //////////////////
            currencyPicker = new Picker
            {
                Title = "Currency",
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Center
            };

            int favouriteIndex = 0;
            int counter = 0;
            foreach (Currency c in db.GetCurrencies())
            {
                if (c.code == selectedCurrency.code)
                    favouriteIndex = counter;
                currencyPicker.Items.Add(c.code);
                counter++;
            }
            currencyPicker.SelectedIndex = favouriteIndex;
            currencyPicker.SelectedIndexChanged += currencyChanged;
        }

        private void currencyChanged(object sender, EventArgs e)
        {
            selectedCurrency = db.GetCurrencies().ElementAt(currencyPicker.SelectedIndex);
            mainGrid.Children.Remove(grid);
            updateGrid();
            mainGrid.Children.Add(grid,1,1);            
        }

        private void updateGrid()
        {
            ////////////////// GRID //////////////////
            grid = new Grid()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            grid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }
            };
            
            grid.Children.Add(new Label()
            {
                Text = "Code", FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center
            }, 0, 0);
            grid.Children.Add(new Label()
            {
                Text = "Rate",
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center
            }, 1, 0);

            int i = 1;
            foreach (Currency c in db.GetCurrencies())
            {
                Label rateCode = new Label()
                {
                    Text = c.code,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                Label rate = new Label()
                {
                    Text = string.Format("{0:0.00000}", c.getRate(selectedCurrency)),
                    HorizontalTextAlignment = TextAlignment.Center
                };
                if(c.code == selectedCurrency.code)
                {
                    rateCode.BackgroundColor = Color.Gray;
                    rate.BackgroundColor = Color.Gray;
                }

                grid.Children.Add(rateCode, 0, i);
                grid.Children.Add(rate, 1, i++);
            }
        }
    }
}
