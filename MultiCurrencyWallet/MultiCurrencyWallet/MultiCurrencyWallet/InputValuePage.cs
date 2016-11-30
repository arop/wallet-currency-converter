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
        Picker actionPicker;
        Label entryLabel;
        Entry valueEntry;
        Button button;

        public InputValuePage()
        {

            actionPicker = new Picker
            {
                Title = "Action",
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            actionPicker.Items.Add("Add");
            actionPicker.Items.Add("Remove");
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

            button = new Button()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "Add",
                BackgroundColor = Utils.addColor
            };
            button.Clicked += OnButtonClicked;

            var stack1 = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = { actionPicker, entryLabel, valueEntry, button },
                HorizontalOptions = LayoutOptions.Center
            };

            Content = new StackLayout{
                Children = { stack1 }
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
