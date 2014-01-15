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
    public partial class MembersPage : PhoneApplicationPage
    {
        public MembersPage() {
            InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void OnClick_AddMemberButton(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("/Pages/MemberNewPage.xaml", UriKind.Relative));
        }
    }
}