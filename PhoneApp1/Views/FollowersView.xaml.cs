using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PhoneApp1.Views
{
    public partial class FollowersView : PhoneApplicationPage
    {
        public FollowersView() {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void OnClick_AddFollowerButton(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("/Views/NewFollowerView.xaml", UriKind.Relative));
        }
    }
}