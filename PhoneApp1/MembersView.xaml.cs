using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp1.Models;
using System.Diagnostics;

namespace PhoneApp1
{
    public partial class MembersView : PhoneApplicationPage
    {
        public MembersView()
        {
            InitializeComponent();
            //this.DataContext = App.ViewModel;
        }

        private void DeleteMemberButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
            }
            this.Focus();
        }

        private void NewMemberAppBarButton_Click(object sender, EventArgs e)
        {
            NewMemberPopup.IsOpen = true;
            ApplicationBar.IsVisible = false;

            DatePicker dp = BirthdayDatePicker as DatePicker;
            if (dp != null && dp.Value != null)
            {
                Debug.WriteLine(dp.Value.ToString());
            }

            this.Focus();
        }

        private void NewMemberAddButton_Click(object sender, RoutedEventArgs e)
        {
            int matnr;
            Int32.TryParse(MatNrTextBox.Text, out matnr);
            string surname = SurnameTextBox.Text;
            string forename = ForenameTextBox.Text;
            DateTime birthday = BirthdayDatePicker.Value.GetValueOrDefault(DateTime.Today);

        }

        private void NewMemberCancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewMemberPopup.IsOpen = false;
            ApplicationBar.IsVisible = true;
        }

        private void BirthdayDatePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            NewMemberPopup.IsOpen = true;
        }
    }
}