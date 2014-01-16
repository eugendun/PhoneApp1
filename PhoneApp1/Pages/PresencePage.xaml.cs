using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PhoneApp1
{
    public partial class PresencePage : PhoneApplicationPage
    {
        public PresencePage() {
            InitializeComponent();
            if (App.Current.Resources.Contains("SelectedLecture")) {
                App.Current.Resources.Remove("SelectedLecture");
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (!App.Current.Resources.Contains("SelectedLecture")) {
                NavigationService.Navigate(new Uri("/Pages/LectureSelectPage.xaml", UriKind.Relative));
            }
            base.OnNavigatedTo(e);
        }
    }
}