using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MultiCurrencyWallet
{
    public class InputValuePage : ContentPage
    {
        Picker actionPicker, currencyPicker;
        Label entryLabel;
        Entry valueEntry;
        Button button;
        DatabaseOps db;

        public InputValuePage(DatabaseOps db)
        {
            this.db = db;
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

            var entryLabel = new Label()
            {
                Text = "Amount:"
            };
            valueEntry = new Entry()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Placeholder = "Amount",
                Keyboard = Keyboard.Numeric
            };


            currencyPicker = new Picker
            {
                Title = "Currency",
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            
            foreach (Currency c in db.GetCurrencies())
            {
                currencyPicker.Items.Add(c.code + " - " + c.rate);
            }
            currencyPicker.SelectedIndex = 0;

            button = new Button()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "Add",
                BackgroundColor = Utils.addColor
            };
            button.Clicked += OnButtonClicked;


            /*var stack1 = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = { actionPicker, entryLabel, valueEntry, button },
                HorizontalOptions = LayoutOptions.Center
            };*/

            var grid = new Grid()
            {
                HorizontalOptions = LayoutOptions.Center
            };

            grid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            };

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            grid.Children.Add(actionPicker, 1, 0);
            grid.Children.Add(entryLabel, 1, 1);
            grid.Children.Add(valueEntry, 1, 2);
            grid.Children.Add(currencyPicker, 1, 3);
            grid.Children.Add(button, 1, 4);

            Content = new StackLayout{
                Children = { grid }
            };

        }

        private void actionChanged(object sender, EventArgs e)
        {
            //Debug.WriteLine("action changed");
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

        void OnButtonClicked(object sender, EventArgs e)
        {

        }

    }
}
