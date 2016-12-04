using System;
using System.Linq;

using Xamarin.Forms;

namespace MultiCurrencyWallet.Pages
{
    public class SetFavouriteCurrencyPage : ContentPage
    {
        Picker currencyPicker;
        Currency selectedCurrency;
        DatabaseOps db;
        Label favouriteCurrencyLabel;
        private InputValuePage inputValuePage;

        public SetFavouriteCurrencyPage(DatabaseOps d)
        {
            db = d;
            string favouriteCurrency = db.GetGlobal("favouriteCurrency");

            this.Title = "Favourite Currency";

            //////////////////////// LABEL ///////////////////
            Label topLabel = new Label()
            {
                Text = "Favourite Currency:",
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                //FontAttributes = FontAttributes.Bold
            };

            favouriteCurrencyLabel = new Label()
            {
                Text = favouriteCurrency,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                FontAttributes = FontAttributes.Bold
            };


            //////////////////////// LABEL ///////////////////
            Label changeFavouriteTitleLabel = new Label()
            {
                Text = "Choose your favourite currency:",
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                //FontAttributes = FontAttributes.Bold
            };

            ////////////////// CURRENCY PICKER //////////////////
            currencyPicker = new Picker
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            int counter = 0;
            int favouriteIndex = 0;
            foreach (Currency c in db.GetCurrencies())
            {
                if (favouriteCurrency == c.code)
                    favouriteIndex = counter;
                currencyPicker.Items.Add(c.code);
                counter++;
            }
            currencyPicker.SelectedIndex = favouriteIndex;
            selectedCurrency = db.GetCurrency("EUR");


            ////////////////// BUTTON //////////////////
            Button saveButton = new Button()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "Save",
                BackgroundColor = Color.Gray
            };
            saveButton.Clicked += OnSaveButtonClicked;


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

            /*for (int i = 0; i < 3; i++)
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });*/

            grid.Children.Add(topLabel, 1, 0);
            grid.Children.Add(favouriteCurrencyLabel, 1, 1);
            grid.Children.Add(changeFavouriteTitleLabel, 1, 2);
            grid.Children.Add(currencyPicker, 1, 3);
            grid.Children.Add(saveButton, 1, 4);



            Content = new StackLayout
            {
                Children = {
                    grid
                }
            };
        }

        public SetFavouriteCurrencyPage(DatabaseOps d, InputValuePage inputValuePage) : this(d)
        {
            this.inputValuePage = inputValuePage;
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            string newCode = db.GetCurrencies().ElementAt(currencyPicker.SelectedIndex).code;
            db.SetGlobal("favouriteCurrency", newCode);
            favouriteCurrencyLabel.Text = newCode;
            inputValuePage.updateFavouriteCurrency();
        }
    }
}
