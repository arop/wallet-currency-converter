using Acr.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MultiCurrencyWallet
{
    public partial class BalancePage : ContentPage
    {
        private double screenWidth, screenHeight, graphMaxHeight;
        private Grid mainGrid, graphGrid;
        private Label totalLabel;
        public Picker currencyPicker;
        private Wallet wallet;
        private DatabaseOps db;
        private Currency selectedCurrency;

        public BalancePage(Wallet w, DatabaseOps d)
        {
            this.Title = "Balance";

            wallet = w;
            db = d;

            screenWidth = DeviceInfo.Hardware.ScreenWidth;
            screenHeight = ConvertPixelsToDp(DeviceInfo.Hardware.ScreenHeight);
            
            Debug.WriteLine("Height: " + screenHeight + " # Width: " + screenWidth);
            graphMaxHeight = screenHeight * 0.6;

            selectedCurrency = db.GetCurrency(db.GetGlobal("favouriteCurrency"));

            InitializeMainGrid();
            InitializeGraph();
            InitializeTotals();
            
            var scrollview = new ScrollView
            {
                Content = new StackLayout
                {
                    Children = { mainGrid }
                }
            };

            Content = new StackLayout
            {
                Children = { scrollview }
            };
        }

        private void InitializeMainGrid()
        {
            mainGrid = new Grid()
            {
                HorizontalOptions = LayoutOptions.Center
            };

            mainGrid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            };

            
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        }

        private void InitializeGraph()
        {

            graphGrid = new Grid()
            {
                HorizontalOptions = LayoutOptions.Center,
                ColumnSpacing = 10,
                Margin = new Thickness(30,0)
            };

            for (int i = 0; i < wallet.Balances.Count; i++)
                graphGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            graphGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(graphMaxHeight, GridUnitType.Absolute) });
            graphGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            graphGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            double walletTotal = wallet.GetTotal("EUR", db);
            double barWidth = (wallet.Balances.Count <= 3 ? screenWidth/5 : screenWidth / wallet.Balances.Count);

            Debug.WriteLine("max: " + graphMaxHeight);
            Debug.WriteLine("wallet total: " + walletTotal);

            for (int i = 0; i < wallet.Balances.Count; i++)
            {
                double barValue = Currency.ConvertAmount(db, wallet.Balances.ElementAt(i).Value, wallet.Balances.ElementAt(i).Key, "EUR");
                BoxView bar = new BoxView
                {
                    Color = Utils.graphBarColors[i],
                    WidthRequest = barWidth,
                    HeightRequest = ( barValue / walletTotal ) * graphMaxHeight,
                    //HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End
                };

                Debug.WriteLine("bar " + i + "->  value: "+ barValue + " # height: " + bar.HeightRequest);

                graphGrid.Children.Add(bar, i, 0);
                graphGrid.Children.Add(new Label() { Text = wallet.Balances.ElementAt(i).Key, HorizontalTextAlignment = TextAlignment.Center }, i, 1);
                graphGrid.Children.Add(new Label() { Text = string.Format("{0:0.00}", wallet.Balances.ElementAt(i).Value), HorizontalTextAlignment = TextAlignment.Center }, i, 2);
            }

            mainGrid.Children.Add(graphGrid, 0, 0);
        }

        private void InitializeTotals()
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

            //////////////////////// LABEL ///////////////////
            totalLabel = new Label()
            {
                Text = "Loading...",
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                FontAttributes = FontAttributes.Bold
            };
            UpdateTotalLabelText();

            var totals = new StackLayout
            {
                Children = { currencyPicker, totalLabel }
            };
            mainGrid.Children.Add(totals, 0, 2);
        }

        private void currencyChanged(object sender, EventArgs e)
        {
            //Debug.WriteLine("action changed");
            selectedCurrency = db.GetCurrencies().ElementAt(((Picker)sender).SelectedIndex);
            UpdateTotalLabelText();
        }

        private void UpdateTotalLabelText()
        {
            string labelText = Math.Round(wallet.GetTotal(selectedCurrency.code, db), 2) + " " + selectedCurrency.code;
            totalLabel.Text = labelText;
        }

        private double ConvertPixelsToDp(float pixelValue)
        {

            double dp = pixelValue;

            if (Device.OS == TargetPlatform.Android)
            {
                //dp = (int) ((pixelValue)/Android.Content.Res.Resources.DisplayMetrics.Density);
                float fdpWidth = (float)App.Current.MainPage.Width;
                double pixPerDp = screenWidth / fdpWidth;
                Debug.WriteLine("pixels per dp: " + pixPerDp);
                dp = pixelValue / pixPerDp;
            }
            else if (Device.OS == TargetPlatform.Windows)
            {

            }
            Debug.WriteLine("dp: " + dp); 
            return dp;
        }
    }
}
