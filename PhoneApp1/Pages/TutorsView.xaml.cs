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

namespace PhoneApp1.Views
{
    public partial class TutorsView : PhoneApplicationPage
    {
        public TutorsView() {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void OnClick_DeleteTutorButton(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            if (button != null) {
                Tutor tutorToRemove = button.DataContext as Tutor;
                App.ViewModel.Tutors.Remove(tutorToRemove);
            }
        }

        private void OnClick_AddNewTutorButton(object sender, EventArgs e) {
            NavigationService.Navigate(new Uri("/Pages/TutorNewPage.xaml", UriKind.Relative));
        }
    }
}